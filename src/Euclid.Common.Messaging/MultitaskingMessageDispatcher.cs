using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Euclid.Common.Logging;
using Euclid.Common.Messaging.Exceptions;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Common.Messaging
{
	public class MultitaskingMessageDispatcher<TRegistry> : IMessageDispatcher, ILoggingSource
		where TRegistry : IPublicationRegistry<IPublicationRecord>
	{
		private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
		private readonly IServiceLocator _container;
		private readonly IPublicationRegistry<IPublicationRecord> _publicationRegistry;
		private IMessageChannel _inputChannel;
		private Task _listenerTask;
		private IList<Type> _messageProcessorTypes;

		public MultitaskingMessageDispatcher(IServiceLocator container, TRegistry publicationRegistry)
		{
			_container = container;
			_publicationRegistry = publicationRegistry;
		}

		public IMessageDispatcherSettings CurrentSettings { get; private set; }
		public MessageDispatcherState State { get; private set; }

		public void Configure(IMessageDispatcherSettings settings)
		{
			if (settings.InputTransport.Value == null)
			{
				throw new NoInputTransportConfiguredException
					(
					"You must specify an input channel for a message dispatcher");
			}

			if (settings.MessageProcessorTypes.Value == null || settings.MessageProcessorTypes.Value.Count == 0)
			{
				throw new NoMessageProcessorsConfiguredException
					(
					"You must specify one or more message processors for a message dispatcher");
			}

			if (settings.DurationOfDispatchingSlice.Value.TotalMilliseconds == 0)
			{
				throw new NoDispatchingSliceDurationConfiguredException
					(
					"You must specify a duration for the dispatcher to dispatch messages during.");
			}

			if (settings.NumberOfMessagesToDispatchPerSlice.Value == 0)
			{
				throw new NoNumberOfMessagesPerSliceConfiguredException
					(
					"You must specify the maximum number of messages to be dispatched during a slice of time.");
			}

			CurrentSettings = settings;

			_inputChannel = settings.InputTransport.Value;
			_messageProcessorTypes = settings.MessageProcessorTypes.Value;

			this.WriteInfoMessage
				(string.Format
				 	("Dispatcher configured with input channel type {0} and {1} message processors.",
				 	 _inputChannel.GetType(), _messageProcessorTypes.Count));
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
			// SELF create a new task which periodically retrieves messages from the input channel
			// and spawns new tasks to process them. each processor should be resolved from the container on demand
			_inputChannel.Open();

			State = MessageDispatcherState.Enabled;

			this.WriteInfoMessage("Dispatcher enabled.");

			_listenerTask = Task.Factory.StartNew(taskMethod => PollRegistryForRecords(), _cancellationToken);
		}

		private void DispatchMessage()
		{
			var records = _inputChannel
				.ReceiveMany(CurrentSettings.NumberOfMessagesToDispatchPerSlice.Value, CurrentSettings.DurationOfDispatchingSlice.Value).Cast<IPublicationRecord>();

			foreach (var record in records)
			{
				try
				{
					var message = _publicationRegistry.GetMessage(record.MessageLocation, record.MessageType);

					//find all processor types that implement IMessageProcessor<T>
					//and get the first where T = record.MessageType
					var messageProcessorType = _messageProcessorTypes
						.Where
						(processorType =>
						 processorType
						 	.GetInterface("IMessageProcessor`1")
						 	.GetGenericArguments()
						 	.Any(type => type == message.GetType()))
						.Select(processorType => processorType)
						.FirstOrDefault();

					//couldn't find the processor type
					if (messageProcessorType == null)
					{
						_publicationRegistry.MarkAsUnableToDispatch
							(record.Identifier, true,
							 string.Format
							 	(
							 	 "Could not find a processor for the message type {0}",
							 	 record.MessageType.FullName));
					}

					ProcessMessage(record.Identifier, message, messageProcessorType);
				}
				catch (ActivationException ae)
				{
					_publicationRegistry.MarkAsUnableToDispatch(record.Identifier, true, ae.Message);
				}
				catch (Exception e)
				{
					_publicationRegistry.MarkAsFailed(record.Identifier, e.Message, e.StackTrace);
				}
			}
		}

		private void PollRegistryForRecords()
		{
			while (!_cancellationToken.IsCancellationRequested)
			{
				Task.Factory.StartNew(dispatchTask => DispatchMessage(), _cancellationToken);

				Thread.Sleep((int) CurrentSettings.DurationOfDispatchingSlice.Value.TotalMilliseconds);
			}
		}

		private void ProcessMessage(Guid recordId, IMessage message, Type messageProcessorType)
		{
			var messageProcessor = _container.GetInstance(messageProcessorType); // throws an activation exception if the type can't be resolved

			Task.Factory.StartNew
				(() =>
				 	{
				 		try
				 		{
				 			//call IMessageProcessor<T>.Process for the given message
				 			var handler = messageProcessorType.GetMethod("Process", new[] {message.GetType()});
				 			handler.Invoke(messageProcessor, new[] {message});

				 			//message handled, mark it in the publicationRegistry
				 			_publicationRegistry.MarkAsComplete(recordId);
				 		}
				 		catch (Exception e)
				 		{
				 			_publicationRegistry.MarkAsFailed
				 				(recordId,
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