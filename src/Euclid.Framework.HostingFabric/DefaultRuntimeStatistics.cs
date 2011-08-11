using System;
using System.Collections.Generic;

namespace Euclid.Framework.HostingFabric
{
	public class DefaultRuntimeStatistics : IFabricRuntimeStatistics
	{
		public DefaultRuntimeStatistics
			(IList<Exception> hostedServiceExceptions, IList<Type> configuredHostedServices, Type configuredServiceHost,
			 FabricRuntimeState runtimeState, IFabricRuntimeSettings settings)
		{
			HostedServiceExceptions = hostedServiceExceptions;
			ConfiguredHostedServices = configuredHostedServices;
			ConfiguredServiceHost = configuredServiceHost;
			RuntimeState = runtimeState;
			Settings = settings;
		}

		public IList<Type> ConfiguredHostedServices { get; private set; }
		public Type ConfiguredServiceHost { get; private set; }
		public IList<Exception> HostedServiceExceptions { get; private set; }
		public FabricRuntimeState RuntimeState { get; private set; }
		public IFabricRuntimeSettings Settings { get; private set; }
	}
}