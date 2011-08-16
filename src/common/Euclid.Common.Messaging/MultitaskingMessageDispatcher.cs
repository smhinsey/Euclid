using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Euclid.Common.Logging;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Common.Messaging
{
	public class MultitaskingMessageDispatcher<TRegistry> : IMessageDispatcher, ILoggingSource
		where TRegistry : IPublicationRegistry<IPublicationRecord>
	{
		private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
		private readonly IServiceLocator _container;
		private readonly IPublicationRegistry<IPublicationRecord> _publicationRegistry;
		private bool _configured;
		private IMessageChannel _inputChannel;
		private IMessageChannel _invalidChannel;
		private Task _listenerTask;
		private IList<IMessageProcessor> _messageProcessors;

		public MultitaskingMessageDispatcher(IServiceLocator container, TRegistry publicationRegistry)
		{
			_container = container;
			_publicationRegistry = publicationRegistry;
		}

		public IMessageDispatcherSettings CurrentSettings { get; private set; }
		public MessageDispatcherState State { get; private set; }

		public void Configure(IMessageDispatcherSettings settings)
		{
			if (settings.InputChannel.Value == null)
			{
				throw new NoInputChannelConfiguredException
					(
					"You must specify an input channel for a message dispatcher");
			}

			if (settings.InvalidChannel.Value == null)
			{
				throw new NoInputChannelConfiguredException("You must specify an invalid message channel for a message dispatcher");
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

			_inputChannel = settings.InputChannel.Value;
			_invalidChannel = settings.InvalidChannel.Value;

			_messageProcessors = new List<IMessageProcessor>();

			foreach (var type in CurrentSettings.MessageProcessorTypes.Value)
			{
				var processor = _container.GetInstance(type) as IMessageProcessor;

				if (processor == null) continue;

				(_messageProcessors as List<IMessageProcessor>).Add(processor);
			}

			this.WriteInfoMessage
				(string.Format
				 	("Dispatcher configured with input channel type {0} and {1} message processors.", _inputChannel.GetType(), _messageProcessors.Count()));

			_configured = true;
		}

		public void Disable()
		{
			dispatcherIsConfigured();

			// stop the input
			_cancellationToken.Cancel();

			State = MessageDispatcherState.Disabled;

			if (_listenerTask != null)
			{
				// wait up to 10 seconds for the listener task to exit gracefully
				_listenerTask.Wait(10000);
			}

			this.WriteInfoMessage("Dispatcher disabled.");
		}

		public void Enable()
		{
			dispatcherIsConfigured();

			_inputChannel.Open();

			_invalidChannel.Open();

			State = MessageDispatcherState.Enabled;

			this.WriteInfoMessage("Dispatcher enabled.");

			_listenerTask = Task.Factory.StartNew(taskMethod => pollChannelForRecords(), _cancellationToken);
		}

		private void dispatchMessage()
		{
			var messages = _inputChannel
				.ReceiveMany(CurrentSettings.NumberOfMessagesToDispatchPerSlice.Value, CurrentSettings.DurationOfDispatchingSlice.Value);

			foreach (var channelMessage in messages)
			{
				var record = channelMessage as IPublicationRecord;

				if (record == null)
				{
					_invalidChannel.Send(channelMessage);
					continue;
				}

				var message = _publicationRegistry.GetMessage(record.MessageLocation, record.MessageType);

				var processors = _messageProcessors.Where(x => x.CanProcessMessage(message));

				if (processors.Count() == 0)
				{
					var msg = string.Format("The dispatcher {0} has no processors configured to handle a message of type {1}", GetType().FullName, message.GetType().FullName);

					this.WriteErrorMessage(msg, null);

					_publicationRegistry.MarkAsUnableToDispatch(record.Identifier, true, msg);

					continue;
				}

				foreach (var messageProcessor in processors)
				{
					var processor = _container.GetInstance(messageProcessor.GetType());

					// SELF if we create these as Tasks that return a value, we can register the results after execution completes
					// freeing us of the need to resolve the registry inside the task

					Task.Factory.StartNew
						(() =>
						 	{
						 		try
						 		{
						 			var handler = processor.GetType().GetMethod("Process", new[] {message.GetType()});

						 			handler.Invoke(processor, new[] {message});

									var registry = (IPublicationRegistry<IPublicationRecord>)_container.GetInstance(typeof(IPublicationRegistry<IPublicationRecord>));

									registry.MarkAsComplete(record.Identifier);

						 			this.WriteInfoMessage("Dispatched message {0} with id {1}.", message.GetType().Name, message.Identifier);
						 		}
						 		catch (Exception e)
						 		{
									this.WriteErrorMessage("An error occurred processing message {0} with id {1}.", e, message.GetType().Name, message.Identifier);

									var registry = (IPublicationRegistry<IPublicationRecord>)_container.GetInstance(typeof(IPublicationRegistry<IPublicationRecord>));

									registry.MarkAsFailed(record.Identifier, e.Message, e.StackTrace);
						 		}
						 	});
				}
			}
		}

		private void dispatcherIsConfigured()
		{
			if (!_configured)
			{
				throw new DispatcherNotConfiguredException(string.Format("The dispatcher {0} has not been configured", GetType().FullName));
			}
		}

		private void pollChannelForRecords()
		{
			while (!_cancellationToken.IsCancellationRequested)
			{
				Task.Factory.StartNew(dispatchTask => dispatchMessage(), _cancellationToken);

				Thread.Sleep((int) CurrentSettings.DurationOfDispatchingSlice.Value.TotalMilliseconds);
			}
		}
	}
}