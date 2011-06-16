using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Castle.Windsor;
using Euclid.Common.Logging;
using Euclid.Common.Registry;

namespace Euclid.Common.Transport
{
    public class MultitaskingMessageDispatcher<TMessage, TRegistry> : IMessageDispatcher, ILoggingSource
        where TMessage : IMessage
        where TRegistry : IRegistry<IRecord>
        {
        private readonly IWindsorContainer _container;
        private IRegistry<IRecord> _registry;
        private Task _listenerTask;
        private IMessageTransport _inputTransport;
        private IList<Type> _messageProcessorTypes;

        public MultitaskingMessageDispatcher(IWindsorContainer container, TRegistry registry)
        {
            _container = container;
            _registry = registry;
        }

        public IMessageDispatcherSettings CurrentSettings { get; private set; }
        public MessageDispatcherState State { get; private set; }

        public void Configure(IMessageDispatcherSettings settings)
        {
            if (settings.InputTransport.Value == null)
            {
                throw new NoInputTransportConfiguredException("You must specify an input transport for a message dispatcher");
            }

            if (settings.MessageProcessorTypes.Value == null || settings.MessageProcessorTypes.Value.Count == 0)
            {
                throw new NoMessageProcessorsConfiguredException("You must specify one or more message processors for a message dispatcher");
            }

            if (settings.DurationOfDispatchingSlice.Value.TotalMilliseconds == 0)
            {
                throw new NoDispatchingSliceDurationConfiguredException("You must specify a duration for the dispatcher to dispatch messages during.");
            }

            if (settings.NumberOfMessagesToDispatchPerSlice.Value == 0)
            {
                throw new NoNumberOfMessagesPerSliceConfiguredException("You must specify the maximum number of messages to be dispatched during a slice of time.");
            }

            CurrentSettings = settings;

            _inputTransport = settings.InputTransport.Value;
            _messageProcessorTypes = settings.MessageProcessorTypes.Value;

            this.WriteInfoMessage
                (string.Format
                    ("Dispatcher configured with input transport type {0} and {1} message processors.",
                     _inputTransport.GetType(), _messageProcessorTypes.Count));
        }

        public void Disable()
        {
            // stop the input

            State = MessageDispatcherState.Disabled;

            this.WriteInfoMessage("Dispatcher disabled.");
        }

        private ConcurrentQueue<IMessage> _messageQueue;

        public void Enable()
        {
            // SELF create a new task which periodically retrieves messages from the input transport
            // and spawns new tasks to process them. each processor should be resolved from the container on demand
            _inputTransport.Open();

            State = MessageDispatcherState.Enabled;

            this.WriteInfoMessage("Dispatcher enabled.");

            _listenerTask = Task.Factory.StartNew(() =>
                                                      {
                                                          var sleepDuration = (int)CurrentSettings.DurationOfDispatchingSlice.Value.TotalMilliseconds*2;
                                                          Thread.Sleep(sleepDuration);

                                                          var messages =
                                                              _inputTransport.ReceiveMany(
                                                                                CurrentSettings.NumberOfMessagesToDispatchPerSlice.Value, 
                                                                                CurrentSettings.DurationOfDispatchingSlice.Value);

                                                          foreach (var message in messages)
                                                          {
                                                              var msg = message;

                                                              var record = _registry.CreateRecord(msg);

                                                              try
                                                              {
                                                                  var messageProcessorType = _messageProcessorTypes
                                                                                                .Where(processorType => processorType.
                                                                                                    GetGenericArguments().Any(messageType=> messageType ==msg.GetType()))
                                                                                                .Select(x => x).FirstOrDefault();

                                                                  var messageProessor =
                                                                      _container.Resolve(messageProcessorType);

                                                                  var handler = messageProcessorType.GetMethod(
                                                                      "Process", new[] {msg.GetType()});
                                                                  handler.Invoke(messageProessor, new[] {msg});

                                                                  _registry.MarkAsComplete(record.Identifier);
                                                              }
                                                              catch (Exception e)
                                                              {
                                                                  _registry.MarkAsFailed(record.Identifier, e.Message, e.StackTrace);
                                                              }
                                                          }

                                                      }
                );

        }
    }
}