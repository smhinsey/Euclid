using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Euclid.Common.Logging;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Common.Messaging
{
	public class MultitaskingMessageDispatcher<TRegistry> : DefaultMessageDispatcher
		where TRegistry : IPublicationRegistry<IPublicationRecord, IPublicationRecord>
	{
		private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

		private readonly IPublicationRegistry<IPublicationRecord, IPublicationRecord> _publicationRegistry;

		private Task _listenerTask;

		public MultitaskingMessageDispatcher(IServiceLocator container, TRegistry publicationRegistry)
		{
			Container = container;
			_publicationRegistry = publicationRegistry;
		}

		public override void Disable()
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

		public override void Enable()
		{
			this.WriteDebugMessage("Begining to enable dispatcher.");

			dispatcherIsConfigured();

			InputChannel.Open();

			InvalidChannel.Open();

			State = MessageDispatcherState.Enabled;

			_listenerTask = Task.Factory.StartNew(taskMethod => pollChannelForRecords(), _cancellationToken);

			this.WriteInfoMessage("Dispatcher enabled.");
		}

		private void dispatchMessage()
		{
			var messages = InputChannel.ReceiveMany(
				CurrentSettings.NumberOfMessagesToDispatchPerSlice.Value, CurrentSettings.DurationOfDispatchingSlice.Value);

			foreach (var channelMessage in messages)
			{
				var record = channelMessage as IPublicationRecord;

				if (record == null)
				{
					InvalidChannel.Send(channelMessage);
					continue;
				}

				var message = _publicationRegistry.GetMessage(record.MessageLocation, record.MessageType);

				var processors = MessageProcessors.Where(x => x.CanProcessMessage(message)).ToList();

				if (processors.Count() == 0)
				{
					var msg = string.Format(
						"The dispatcher {0} has no processors configured to handle a message of type {1}",
						GetType().FullName,
						message.GetType().FullName);

					this.WriteErrorMessage(msg, "");

					_publicationRegistry.MarkAsUnableToDispatch(record.Identifier, true, msg);

					continue;
				}

				foreach (var messageProcessor in processors)
				{
					var processor = Container.GetInstance(messageProcessor.GetType());

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
										Container.GetInstance(typeof(IPublicationRegistry<IPublicationRecord, IPublicationRecord>));

									registry.MarkAsComplete(record.Identifier);

									this.WriteInfoMessage("Dispatched message {0} with id {1}.", message.GetType().Name, message.Identifier);
								}
								catch (Exception e)
								{
									this.WriteErrorMessage(
										"An error occurred processing message {0} with id {1}.",
										e.InnerException,
										message.GetType().Name,
										message.Identifier);

									var registry =
										(IPublicationRegistry<IPublicationRecord, IPublicationRecord>)
										Container.GetInstance(typeof(IPublicationRegistry<IPublicationRecord, IPublicationRecord>));

									registry.MarkAsFailed(record.Identifier, e.InnerException.Message, e.InnerException.StackTrace);
								}
							});
				}
			}
		}

		private void dispatcherIsConfigured()
		{
			if (!Configured)
			{
				throw new DispatcherNotConfiguredException(
					string.Format("The dispatcher {0} has not been configured", GetType().FullName));
			}
		}

		private void pollChannelForRecords()
		{
			while (!_cancellationToken.IsCancellationRequested)
			{
				Task.Factory.StartNew(dispatchTask => dispatchMessage(), _cancellationToken);

				Thread.Sleep((int)CurrentSettings.DurationOfDispatchingSlice.Value.TotalMilliseconds);

				this.WriteDebugMessage("Polling for records");
			}

			this.WriteDebugMessage("Stopped polling for records");
		}
	}
}