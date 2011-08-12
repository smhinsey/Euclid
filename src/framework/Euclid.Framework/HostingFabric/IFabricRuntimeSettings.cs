using System;
using Euclid.Common.Configuration;
using Euclid.Common.Messaging;

namespace Euclid.Framework.HostingFabric
{
	public interface IFabricRuntimeSettings : IOverridableSettings
	{
		IOverridableSettingList<Type> HostedServices { get; set; }
		IOverridableSetting<Type> ServiceHost { get; set; }
		IOverridableSetting<IMessageChannel> InputChannel { get; set; }
		IOverridableSetting<IMessageChannel> ErrorChannel { get; set; }
	}
}