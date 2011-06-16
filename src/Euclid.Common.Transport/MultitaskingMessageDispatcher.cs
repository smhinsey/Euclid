using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using Euclid.Common.Logging;
using Euclid.Common.Registry;

namespace Euclid.Common.Transport
{
	// TMessage never turns out to be needed, maybe we can remove it?
	public class MultitaskingMessageDispatcher<TMessage, TRegistry> : IMessageDispatcher, ILoggingSource
		where TMessage : IMessage
		where TRegistry : IRegistry<IRecord>
	{
		private readonly IWindsorContainer _container;
		private readonly IRegistry<IRecord> _registry;
		private IMessageTransport _inputTransport;
		private Task _listenerTask;
		private IList<Type> _messageProcessorTypes;
		private ConcurrentQueue<IMessage> _messageQueue;

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
			// cancel the listener task's cancellation token via its source

			State = MessageDispatcherState.Disabled;

			this.WriteInfoMessage("Dispatcher disabled.");
		}

		public void Enable()
		{
			// SELF create a new task which periodically retrieves messages from the input transport
			// and spawns new tasks to process them. each processor should be resolved from the container on demand
			_inputTransport.Open();

			State = MessageDispatcherState.Enabled;

			this.WriteInfoMessage("Dispatcher enabled.");

			// we want to keep a cancellation token source around
			// so we can shut this down via disable
			_listenerTask = Task.Factory.StartNew(startSlicingTime);
		}

		private void dispatchSliceOfMessages()
		{
			var messages =
				_inputTransport.ReceiveMany
					(
					 CurrentSettings.NumberOfMessagesToDispatchPerSlice.Value,
					 CurrentSettings.DurationOfDispatchingSlice.Value);

			foreach (var message in messages)
			{
				// i don't think we're capturing over a closure here, so why the local copy?
				var msg = message;

				var record = _registry.CreateRecord(msg);

				try
				{
					// i recommend replacing this with a loop or some other approach. the linq is way shorter but a huge hassle to debug
					// static reflection might be ideal
					var messageProcessorType = _messageProcessorTypes
						.Where(processorType => processorType.GetGenericArguments()
							.Any(messageType => messageType == msg.GetType()))
						.Select(x => x).FirstOrDefault();

					var messageProessor =
						_container.Resolve(messageProcessorType);

					var handler = messageProcessorType
						.GetMethod("Process", new[] {msg.GetType()});
					handler.Invoke(messageProessor, new[] {msg});

					_registry.MarkAsComplete(record.Identifier);
				}
				catch (Exception e)
				{
					_registry.MarkAsFailed(record.Identifier, e.Message, e.StackTrace);
				}
			}
		}

		private void startSlicingTime()
		{
			// begin loop

			// execute on a separate thread
			dispatchSliceOfMessages();

			// wait the duration of the slice

			// continue loop
		}
	}
}