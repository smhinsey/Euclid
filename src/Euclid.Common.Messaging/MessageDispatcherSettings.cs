using System;
using Euclid.Common.Configuration;

namespace Euclid.Common.Messaging
{
	public class MessageDispatcherSettings : IMessageDispatcherSettings
	{
		public MessageDispatcherSettings()
		{
			InputTransport = new OverridableSetting<IMessageChannel>();
			MessageProcessorTypes = new OverridableSettingList<Type>();
			NumberOfMessagesToDispatchPerSlice = new OverridableSetting<int>();
			DurationOfDispatchingSlice = new OverridableSetting<TimeSpan>();
		}

		public IOverridableSetting<TimeSpan> DurationOfDispatchingSlice { get; set; }

		public IOverridableSetting<IMessageChannel> InputTransport { get; set; }
		public IOverridableSettingList<Type> MessageProcessorTypes { get; set; }
		public IOverridableSetting<int> NumberOfMessagesToDispatchPerSlice { get; set; }
	}
}