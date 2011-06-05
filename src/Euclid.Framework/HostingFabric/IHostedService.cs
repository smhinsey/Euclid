namespace Euclid.Framework.HostingFabric
{
	/// <summary>
	/// An instance of IHostedService encapsulates the hosting logic required by a particular
	/// service, such as an agent, a data store, a cache, etc.
	/// </summary>
	public interface IHostedService
	{
		string Name { get; }
		HostedServiceState State { get; }
		void Start();
		void Stop();
		/// <summary>
		/// Pause is invoked by the fabric controller prior to migrating the service
		/// from one fabric runtime to another.
		/// </summary>
		void Pause();
	}
}