namespace Euclid.Framework.HostingFabric
{
	/// <summary>
	/// The direct implementation of the requirements of the runtime fabric for a specific
	/// runtime environment such as Azure, EC2, etc.
	/// </summary>
	public interface IFabricRuntime
	{
		string InstallFabricService();
		void StartFabricService(string id);
		void PauseFabricService(string id);
		void TerminateFabricService(string id);
		void IncreaseFabricServiceCapacity(string id);
		void DecreaseFabricServiceCapacity(string id);
		FabricServiceState GetFabricServiceState(string id);
	}
}