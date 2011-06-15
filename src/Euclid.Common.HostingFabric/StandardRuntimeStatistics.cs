using System;
using System.Collections.Generic;

namespace Euclid.Common.HostingFabric
{
	public class StandardRuntimeStatistics : IFabricRuntimeStatistics
	{
		public IList<Type> ConfiguredHostedServices { get; private set; }
		public IList<Type> ConfiguredServiceHosts { get; private set; }
		public FabricRuntimeState RuntimeState { get; private set; }
		public IFabricRuntimeSettings Settings { get; private set; }
	}
}