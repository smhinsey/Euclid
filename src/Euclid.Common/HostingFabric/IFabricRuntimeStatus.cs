using System;
using System.Collections.Generic;
using Euclid.Common.Configuration;

namespace Euclid.Common.HostingFabric
{
	public interface IFabricRuntimeStatus
	{
		FabricRuntimeState RuntimeState { get; }
		IList<Type> ConfiguredHostedServices { get; }
		IList<Type> ConfiguredServiceHosts { get; }
		IFabricRuntimeSettings Settings { get; }
	}

	public interface IFabricRuntimeSettings : IOverridableSettings
	{
	}
}