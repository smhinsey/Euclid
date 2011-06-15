using System;
using Euclid.Common.Configuration;

namespace Euclid.Common.Transport
{
	public class MessageDispatcherSettings : IMessageDispatcherSettings
	{
		public MessageDispatcherSettings()
		{
			InputTransport = new OverridableSetting<IMessageTransport>();
			MessageProcessorTypes = new OverridableSettingList<Type>();
			NumberOfMessagesToDispatchPerSlice = new OverridableSetting<int>();
			DurationOfDispatchingSlice = new OverridableSetting<TimeSpan>();
		}

		public IOverridableSetting<TimeSpan> DurationOfDispatchingSlice { get; set; }

		public IOverridableSetting<IMessageTransport> InputTransport { get; set; }
		public IOverridableSettingList<Type> MessageProcessorTypes { get; set; }
		public IOverridableSetting<int> NumberOfMessagesToDispatchPerSlice { get; set; }
	}
}