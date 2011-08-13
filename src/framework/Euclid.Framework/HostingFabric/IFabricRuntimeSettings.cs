using System;
using Euclid.Common.Configuration;
using Euclid.Common.Messaging;

namespace Euclid.Framework.HostingFabric
{
	public interface IFabricRuntimeSettings : IOverridableSettings
	{
		IOverridableSetting<IMessageChannel> ErrorChannel { get; set; }
		IOverridableSettingList<Type> HostedServices { get; set; }
		IOverridableSetting<IMessageChannel> InputChannel { get; set; }
		IOverridableSetting<Type> ServiceHost { get; set; }
	}
}