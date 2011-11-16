using System.Collections.Generic;
using System.Linq;
using Euclid.Common.Logging;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Common.Messaging
{
	public abstract class DefaultMessageDispatcher : IMessageDispatcher, ILoggingSource
	{
		protected bool Configured;

		protected IServiceLocator Container;

		protected IMessageChannel InputChannel;

		protected IMessageChannel InvalidChannel;

		protected IList<IMessageProcessor> MessageProcessors;

		public IMessageDispatcherSettings CurrentSettings { get; protected set; }

		public MessageDispatcherState State { get; protected set; }

		public virtual void Configure(IMessageDispatcherSettings settings)
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

			CurrentSettings = settings;

			InputChannel = settings.InputChannel.Value;
			InvalidChannel = settings.InvalidChannel.Value;

			MessageProcessors = new List<IMessageProcessor>();

			foreach (var type in CurrentSettings.MessageProcessorTypes.Value)
			{
				var processor = Container.GetInstance(type) as IMessageProcessor;

				if (processor == null)
				{
					continue;
				}

				this.WriteInfoMessage(string.Format("Adding processor {0} to dispatcher.", processor.GetType().Name));

				MessageProcessors.Add(processor);
			}

			this.WriteInfoMessage(
				string.Format(
					"Dispatcher configured with input channel {0}({1}) and {2} message processors.",
					InputChannel.GetType().Name,
					InputChannel.ChannelName,
					MessageProcessors.Count()));

			Configured = true;
		}

		public abstract void Disable();

		public abstract void Enable();
	}
}