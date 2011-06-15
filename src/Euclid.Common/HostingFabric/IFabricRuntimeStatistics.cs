using System;
using System.Collections.Generic;

namespace Euclid.Common.HostingFabric
{
	public interface IFabricRuntimeStatistics
	{
		IList<Type> ConfiguredHostedServices { get; }
		Type ConfiguredServiceHost { get; }
		FabricRuntimeState RuntimeState { get; }
		IFabricRuntimeSettings Settings { get; }
	}
}