using System;
using Euclid.Common.ServiceHost;

namespace Euclid.Common.RuntimeFabric
{
	public abstract class DefaultFabricController : IFabricController
	{
		private readonly IServiceHost _instanceHost;

		protected DefaultFabricController(IServiceHost instanceHost)
		{
			_instanceHost = instanceHost;
		}

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