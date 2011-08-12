namespace Euclid.Common.ServiceHost
{
	/// <summary>
	/// 	An instance of IHostedService encapsulates the hosting logic required by a particular
	/// 	service, such as an agent, a data store, a cache, etc.
	/// </summary>
	public interface IHostedService
	{
		string Name { get; }
		HostedServiceState State { get; }

		void Cancel();
		void Start();
	}
}