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
		/// <summary>
		/// PauseFabricService is invoked by the fabric controller prior to migrating the service
		/// from one fabric runtime to another.
		/// </summary>
		void Pause();
	}
}