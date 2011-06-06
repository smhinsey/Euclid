using System;
using Euclid.Framework.Common.Hosting;

namespace Euclid.Framework.Common.Fabric
{
	public class DefaultFabricController : IFabricController
	{
		private readonly IServiceHost _instanceHost;

		public DefaultFabricController(IServiceHost instanceHost)
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