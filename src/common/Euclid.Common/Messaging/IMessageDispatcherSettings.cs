using System;
using Euclid.Common.Configuration;

namespace Euclid.Common.Messaging
{
	public interface IMessageDispatcherSettings : IOverridableSettings
	{
		IOverridableSetting<TimeSpan> DurationOfDispatchingSlice { get; }

		IOverridableSetting<IMessageChannel> InputChannel { get; }

		IOverridableSetting<IMessageChannel> InvalidChannel { get; }

		IOverridableSettingList<Type> MessageProcessorTypes { get; }

		IOverridableSetting<int> NumberOfMessagesToDispatchPerSlice { get; }
	}
}