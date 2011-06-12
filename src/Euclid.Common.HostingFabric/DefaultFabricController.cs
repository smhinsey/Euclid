using System;

namespace Euclid.Common.HostingFabric
{
	public abstract class DefaultFabricController : IFabricController
	{
		public FabricControllerState State { get; protected set; }

		public void Start()
		{
			throw new NotImplementedException();
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}
	}
}