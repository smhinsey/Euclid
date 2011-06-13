using System;

namespace Euclid.Common.HostingFabric
{
	public class LocalMachineFabricRuntime : IFabricRuntime
	{
		public FabricRuntimeState State { get; private set; }

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