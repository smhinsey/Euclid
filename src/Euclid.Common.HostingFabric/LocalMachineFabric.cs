using System;

namespace Euclid.Common.HostingFabric
{
	public class LocalMachineFabric : IFabricRuntime
	{
		public LocalMachineFabric()
		{
			State = FabricRuntimeState.Stopped;
			Statistics = new StandardRuntimeStatistics();
		}

		public FabricRuntimeState State { get; private set; }
		public IFabricRuntimeStatistics Statistics { get; private set; }

		public void Configure(IFabricRuntimeSettings settings)
		{
			throw new NotImplementedException();
		}

		public void Shutdown()
		{
			throw new NotImplementedException();
		}

		public void Start()
		{
			throw new NotImplementedException();
		}
	}
}