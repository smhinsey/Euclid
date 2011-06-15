using System;
using Euclid.Common.Configuration;

namespace Euclid.Common.HostingFabric
{
	public interface IFabricRuntimeSettings : IOverridableSettings
	{
		IOverridableSettingList<Type> HostedServices { get; set; }
		IOverridableSetting<Type> ServiceHost { get; set; }
	}
}