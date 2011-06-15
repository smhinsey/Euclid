using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Windsor;
using Euclid.Common.Logging;

namespace Euclid.Common.Transport
{
	public class MultitaskingMessageDispatcher : IMessageDispatcher, ILoggingSource
	{
		private readonly IWindsorContainer _container;
		private IMessageTransport _inputTransport;
		private IList<Type> _messageProcessorTypes;
		private Task _inputTask;

		public IMessageDispatcherSettings CurrentSettings { get; private set; }
		public MessageDispatcherState State { get; private set; }

		public MultitaskingMessageDispatcher(IWindsorContainer container)
		{
			_container = container;
		}

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
			_inputTransport.Close();

			State = MessageDispatcherState.Disabled;

			this.WriteInfoMessage("Dispatcher disabled.");
		}

		public void Enable()
		{
			_inputTask = new Task(() => _inputTransport.Open());

			State = MessageDispatcherState.Enabled;

			this.WriteInfoMessage("Dispatcher enabled.");
		}
	}
}