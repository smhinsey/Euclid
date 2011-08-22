using System;
using Euclid.Common.Configuration;

namespace Euclid.Common.Messaging
{
	public class MessageDispatcherSettings : IMessageDispatcherSettings
	{
		public MessageDispatcherSettings()
		{
			this.InvalidChannel = new OverridableSetting<IMessageChannel>();
			this.InputChannel = new OverridableSetting<IMessageChannel>();
			this.MessageProcessorTypes = new OverridableSettingList<Type>();
			this.NumberOfMessagesToDispatchPerSlice = new OverridableSetting<int>();
			this.DurationOfDispatchingSlice = new OverridableSetting<TimeSpan>();

			this.NumberOfMessagesToDispatchPerSlice.WithDefault(32);
			this.DurationOfDispatchingSlice.WithDefault(TimeSpan.Parse("00:00:01"));
		}

		public IOverridableSetting<TimeSpan> DurationOfDispatchingSlice { get; set; }

		public IOverridableSetting<IMessageChannel> InputChannel { get; set; }

		public IOverridableSetting<IMessageChannel> InvalidChannel { get; set; }

		public IOverridableSettingList<Type> MessageProcessorTypes { get; set; }

		public IOverridableSetting<int> NumberOfMessagesToDispatchPerSlice { get; set; }
	}
}