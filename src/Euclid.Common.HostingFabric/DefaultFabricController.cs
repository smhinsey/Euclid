using System;
using System.Collections.Generic;
using Euclid.Common.ServiceHost;

namespace Euclid.Common.HostingFabric
{
	public class DefaultFabricController : IFabricController
	{
		public DefaultFabricController()
		{
			ServiceHostTypes = new List<Type>();
			State = FabricControllerState.Stopped;
		}

		public IList<Type> ServiceHostTypes { get; private set; }
		public FabricControllerState State { get; protected set; }

		public void InstallServiceHost(IServiceHost serviceHost)
		{
			throw new NotImplementedException();
		}

		public void StartServiceHosts()
		{
			State = FabricControllerState.Started;

			throw new NotImplementedException();
		}

		public void StopServiceHosts()
		{
			State = FabricControllerState.Stopped;

			throw new NotImplementedException();
		}
	}
}