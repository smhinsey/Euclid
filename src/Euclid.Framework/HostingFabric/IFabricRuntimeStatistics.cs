using System;
using System.Collections.Generic;

namespace Euclid.Framework.HostingFabric
{
	public interface IFabricRuntimeStatistics
	{
		IList<Type> ConfiguredHostedServices { get; }
		Type ConfiguredServiceHost { get; }
		IList<Exception> HostedServiceExceptions { get; }
		FabricRuntimeState RuntimeState { get; }
		IFabricRuntimeSettings Settings { get; }
	}
}