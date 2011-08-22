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
		where TRegistry : IPublicationRegistry<IPublicationRecord, IPublicationRecord>
	{
		private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

		private readonly IServiceLocator _container;

		private readonly IPublicationRegistry<IPublicationRecord, IPublicationRecord> _publicationRegistry;

		private bool _configured;

		private IMessageChannel _inputChannel;

		private IMessageChannel _invalidChannel;

		private Task _listenerTask;

		private IList<IMessageProcessor> _messageProcessors;

		public MultitaskingMessageDispatcher(IServiceLocator container, TRegistry publicationRegistry)
		{
			this._container = container;
			this._publicationRegistry = publicationRegistry;
		}

		public IMessageDispatcherSettings CurrentSettings { get; private set; }

		public MessageDispatcherState State { get; private set; }

		public void Configure(IMessageDispatcherSettings settings)
		{
			if (settings.InputChannel.Value == null)
			{
				throw new NoInputChannelConfiguredException("You must specify an input channel for a message dispatcher");
			}

			if (settings.InvalidChannel.Value == null)
			{
				throw new NoInputChannelConfiguredException("You must specify an invalid message channel for a message dispatcher");
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

			this.CurrentSettings = settings;

			this._inputChannel = settings.InputChannel.Value;
			this._invalidChannel = settings.InvalidChannel.Value;

			this._messageProcessors = new List<IMessageProcessor>();

			foreach (var type in this.CurrentSettings.MessageProcessorTypes.Value)
			{
				var processor = this._container.GetInstance(type) as IMessageProcessor;

				if (processor == null)
				{
					continue;
				}

				(this._messageProcessors as List<IMessageProcessor>).Add(processor);
			}

			this.WriteInfoMessage(
				string.Format(
					"Dispatcher configured with input channel {0}({1}) and {2} message processors.", 
					this._inputChannel.GetType().Name, 
					this._inputChannel.ChannelName, 
					this._messageProcessors.Count()));

			this._configured = true;
		}

		public void Disable()
		{
			this.dispatcherIsConfigured();

			// stop the input
			this._cancellationToken.Cancel();

			this.State = MessageDispatcherState.Disabled;

			if (this._listenerTask != null)
			{
				// wait up to 10 seconds for the listener task to exit gracefully
				this._listenerTask.Wait(10000);
			}

			this.WriteInfoMessage("Dispatcher disabled.");
		}

		public void Enable()
		{
			this.WriteDebugMessage("Begining to enable dispatcher.");

			this.dispatcherIsConfigured();

			this._inputChannel.Open();

			this._invalidChannel.Open();

			this.State = MessageDispatcherState.Enabled;

			this._listenerTask = Task.Factory.StartNew(taskMethod => this.pollChannelForRecords(), this._cancellationToken);

			this.WriteInfoMessage("Dispatcher enabled.");
		}

		private void dispatchMessage()
		{
			var messages = this._inputChannel.ReceiveMany(
				this.CurrentSettings.NumberOfMessagesToDispatchPerSlice.Value, this.CurrentSettings.DurationOfDispatchingSlice.Value);

			foreach (var channelMessage in messages)
			{
				var record = channelMessage as IPublicationRecord;

				if (record == null)
				{
					this._invalidChannel.Send(channelMessage);
					continue;
				}

				var message = this._publicationRegistry.GetMessage(record.MessageLocation, record.MessageType);

				var processors = this._messageProcessors.Where(x => x.CanProcessMessage(message));

				if (processors.Count() == 0)
				{
					var msg = string.Format(
						"The dispatcher {0} has no processors configured to handle a message of type {1}", 
						this.GetType().FullName, 
						message.GetType().FullName);

					this.WriteErrorMessage(msg, null);

					this._publicationRegistry.MarkAsUnableToDispatch(record.Identifier, true, msg);

					continue;
				}

				foreach (var messageProcessor in processors)
				{
					var processor = this._container.GetInstance(messageProcessor.GetType());

					// SELF if we create these as Tasks that return a value, we can register the results after execution completes
					// freeing us of the need to resolve the registry inside the task. The task should look something like:
					// var task = new Task<MessageDispatchResult>({ try{...} catch(Exception e) { return new MessageDispatchResult { Failed = true, Error = e} ; }})

					Task.Factory.StartNew(
						() =>
							{
								try
								{
									var handler = processor.GetType().GetMethod("Process", new[] { message.GetType() });

									handler.Invoke(processor, new[] { message });

									var registry =
										(IPublicationRegistry<IPublicationRecord, IPublicationRecord>)
										this._container.GetInstance(typeof(IPublicationRegistry<IPublicationRecord, IPublicationRecord>));

									registry.MarkAsComplete(record.Identifier);

									this.WriteInfoMessage("Dispatched message {0} with id {1}.", message.GetType().Name, message.Identifier);
								}
								catch (Exception e)
								{
									this.WriteErrorMessage(
										"An error occurred processing message {0} with id {1}.", e, message.GetType().Name, message.Identifier);
									var registry =
										(IPublicationRegistry<IPublicationRecord, IPublicationRecord>)
										this._container.GetInstance(typeof(IPublicationRegistry<IPublicationRecord, IPublicationRecord>));
									registry.MarkAsFailed(record.Identifier, e.Message, e.StackTrace);
								}
							});
				}
			}
		}

		private void dispatcherIsConfigured()
		{
			if (!this._configured)
			{
				throw new DispatcherNotConfiguredException(
					string.Format("The dispatcher {0} has not been configured", this.GetType().FullName));
			}
		}

		private void pollChannelForRecords()
		{
			while (!this._cancellationToken.IsCancellationRequested)
			{
				Task.Factory.StartNew(dispatchTask => this.dispatchMessage(), this._cancellationToken);

				Thread.Sleep((int)this.CurrentSettings.DurationOfDispatchingSlice.Value.TotalMilliseconds);
			}
		}
	}
}