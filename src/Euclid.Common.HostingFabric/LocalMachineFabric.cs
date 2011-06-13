using System;

namespace Euclid.Common.HostingFabric
{
	public class LocalMachineFabric : IFabricRuntime
	{
		public FabricRuntimeState State { get; private set; }

		public void Initialize()
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