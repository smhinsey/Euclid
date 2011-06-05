using System;

namespace Euclid.Framework.HostingFabric
{
	public class ThreadedFabricRuntime : IFabricRuntime
	{
		public string InstallFabricService()
		{
			throw new NotImplementedException();
		}

		public void StartFabricService(string id)
		{
			throw new NotImplementedException();
		}

		public void GetInstanceState(string id)
		{
			throw new NotImplementedException();
		}

		public void PauseFabricService(string id)
		{
			throw new NotImplementedException();
		}

		public void TerminateFabricService(string id)
		{
			throw new NotImplementedException();
		}

		public void IncreaseFabricServiceCapacity(string id)
		{
			throw new NotImplementedException();
		}

		public void DecreaseFabricServiceCapacity(string id)
		{
			throw new NotImplementedException();
		}

		public FabricServiceState GetFabricServiceState(string id)
		{
			throw new NotImplementedException();
		}
	}
}