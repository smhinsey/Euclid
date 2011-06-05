namespace Euclid.Framework.HostingFabric
{
	/// <summary>
	/// An instance of IFabricService encapsulates the hosting logic required by a particular
	/// service, such as an agent, a data store, a cache, etc.
	/// </summary>
	public interface IFabricService
	{
		string Name { get; }
		FabricServiceState State { get; }
		void Start();
		void Stop();
	}
}