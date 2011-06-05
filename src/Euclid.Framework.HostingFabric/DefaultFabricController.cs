using System;

namespace Euclid.Framework.HostingFabric
{
	public class DefaultFabricController : IFabricController
	{
		private readonly IServiceHost _instanceHost;
		public FabricControllerState State { get; protected set; }

		public DefaultFabricController(IServiceHost instanceHost)
		{
			_instanceHost = instanceHost;
		}

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