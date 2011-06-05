using System;

namespace Euclid.Framework.HostingFabric
{
	public class DefaultFabricController : IFabricController
	{
		private readonly IFabricRuntime _runtime;
		public FabricControllerState State { get; protected set; }

		public DefaultFabricController(IFabricRuntime runtime)
		{
			_runtime = runtime;
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