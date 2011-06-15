using System;
using System.Collections.Generic;

namespace Euclid.Common.HostingFabric
{
	public interface IFabricRuntimeStatistics
	{
		IList<Type> ConfiguredHostedServices { get; }
		IList<Type> ConfiguredServiceHosts { get; }
		FabricRuntimeState RuntimeState { get; }
		IFabricRuntimeSettings Settings { get; }
	}
}