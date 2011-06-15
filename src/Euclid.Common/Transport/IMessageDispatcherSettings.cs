using System;
using Euclid.Common.Configuration;

namespace Euclid.Common.Transport
{
	public interface IMessageDispatcherSettings : IOverridableSettings
	{
		IOverridableSetting<TimeSpan> DurationOfDispatchingSlice { get; }
		IOverridableSetting<IMessageTransport> InputTransport { get; }
		IOverridableSettingList<Type> MessageProcessorTypes { get; }
		IOverridableSetting<int> NumberOfMessagesToDispatchPerSlice { get; }
	}
}