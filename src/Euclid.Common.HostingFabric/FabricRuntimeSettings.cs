using System;
using Euclid.Common.Configuration;

namespace Euclid.Common.HostingFabric
{
	public class FabricRuntimeSettings : IFabricRuntimeSettings
	{
		public FabricRuntimeSettings()
		{
			HostedServices = new OverridableSettingList<Type>();
			ServiceHost = new OverridableSetting<Type>();
		}

		public IOverridableSettingList<Type> HostedServices { get; set; }
		public IOverridableSetting<Type> ServiceHost { get; set; }
	}
}