using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.Windsor;
using Euclid.Common.Logging;
using Euclid.Common.Registry;

namespace Euclid.Common.Transport
{
    public class MultitaskingMessageDispatcher<TRegistry> : IMessageDispatcher, ILoggingSource
        where TRegistry : IRegistry<IRecord>
    {
        private readonly IWindsorContainer _container;
        private readonly IRegistry<IRecord> _registry;
        private IMessageTransport _inputTransport;
        private Task _listenerTask;
        private IList<Type> _messageProcessorTypes;

        public MultitaskingMessageDispatcher(IWindsorContainer container, TRegistry registry)
        {
            _container = container;
            _registry = registry;
        }

        public IMessageDispatcherSettings CurrentSettings { get; private set; }
        public MessageDispatcherState State { get; private set; }
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public void Configure(IMessageDispatcherSettings settings)
        {
            if (settings.InputTransport.Value == null)
            {
                throw new NoInputTransportConfiguredException(
                    "You must specify an input transport for a message dispatcher");
            }

            if (settings.MessageProcessorTypes.Value == null || settings.MessageProcessorTypes.Value.Count == 0)
            {
                throw new NoMessageProcessorsConfiguredException(
                    "You must specify one or more message processors for a message dispatcher");
            }

            if (settings.DurationOfDispatchingSlice.Value.TotalMilliseconds == 0)
            {
                throw new NoDispatchingSliceDurationConfiguredException(
                    "You must specify a duration for the dispatcher to dispatch messages during.");
            }

            if (settings.NumberOfMessagesToDispatchPerSlice.Value == 0)
            {
                throw new NoNumberOfMessagesPerSliceConfiguredException(
                    "You must specify the maximum number of messages to be dispatched during a slice of time.");
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
            _cancellationToken.Cancel();

            State = MessageDispatcherState.Disabled;

            // wait up to 10 seconds for the listener task to exit gracefully
            _listenerTask.Wait(10000);

            this.WriteInfoMessage("Dispatcher disabled.");
        }

        public void Enable()
        {
            // SELF create a new task which periodically retrieves messages from the input transport
            // and spawns new tasks to process them. each processor should be resolved from the container on demand
            _inputTransport.Open();

            State = MessageDispatcherState.Enabled;

            this.WriteInfoMessage("Dispatcher enabled.");

            _listenerTask = Task.Factory.StartNew(taskMethod => PollRegistryForRecords(), _cancellationToken);
        }

        private void PollRegistryForRecords()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                Task.Factory.StartNew(dispatchTask => DispatchMessage(), _cancellationToken);

                Thread.Sleep((int)CurrentSettings.DurationOfDispatchingSlice.Value.TotalMilliseconds);
            }
        }

        private void DispatchMessage()
        {
            var records = _inputTransport
                .ReceiveMany<IRecord>(CurrentSettings.NumberOfMessagesToDispatchPerSlice.Value,
                                      CurrentSettings.DurationOfDispatchingSlice.Value);

            foreach (var record in records)
            {
                try
                {
                    var message = _registry.GetMessage(record.MessageLocation, record.MessageType);

                    //find all processor types that implement IMessageProcessor<T>
                    //and get the first where T = record.MessageType
                    var messageProcessorType = _messageProcessorTypes
                                                    .Where(processorType =>
                                                           processorType
                                                               .GetInterface("IMessageProcessor`1")
                                                               .GetGenericArguments()
                                                               .Any(type => type == message.GetType()))
                                                    .Select(processorType => processorType)
                                                    .FirstOrDefault();

                    //couldn't find the processor type
                    if (messageProcessorType == null)
                    {
                        _registry.MarkAsUnableToDispatch(record.Identifier, true,
                                                         string.Format(
                                                             "Could not find a processor for the message type {0}",
                                                             record.MessageType.FullName));
                    }

                    ProcessMessage(record.Identifier, message, messageProcessorType);
                }
                catch (Exception e)
                {
                    _registry.MarkAsFailed(record.Identifier, e.Message, e.StackTrace);
                }
            }
        }

        private void ProcessMessage(Guid recordId, IMessage message, Type messageProcessorType)
        {
            var messageProcessor = _container.Resolve(messageProcessorType);

            //couldn't resolve the processor type
            if (messageProcessor == null)
            {
                _registry.MarkAsUnableToDispatch(recordId, true,
                                                 string.Format(
                                                     "Unable to resolve a processor of type {0}, please ensure it has been registered with the container",
                                                     messageProcessorType.FullName));

                return;
            }

            Task.Factory.StartNew(() =>
                                              {
                                                  try
                                                  {
                                                      //call IMessageProcessor<T>.Process for the given message
                                                      var handler = messageProcessorType.GetMethod("Process", new[] { message.GetType() });
                                                      handler.Invoke(messageProcessor, new[] { message });

                                                      //message handled, mark it in the registry
                                                      _registry.MarkAsComplete(recordId);
                                                  }
                                                  catch (Exception e)
                                                  {
                                                      _registry.MarkAsFailed(recordId,
                                                                             e.Message, e.StackTrace);
                                                  }

                                              });
        }
    }

    public class MessageDispatcherException : Exception
    {
        public MessageDispatcherException(string message)
            : base(message)
        {
        }
    }
}