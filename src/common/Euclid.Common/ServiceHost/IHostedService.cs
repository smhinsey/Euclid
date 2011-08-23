namespace Euclid.Common.ServiceHost
{
	/// <summary>
	/// 	An instance of IHostedService encapsulates the hosting logic required by a particular
	/// 	service, such as an agent, a data store, a cache, etc.
	/// </summary>
	public interface IHostedService
	{
		/// <summary>
		/// Gets the hosted service's friendly name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the current state of the hosted service.
		/// </summary>
		HostedServiceState State { get; }

		/// <summary>
		/// Cancels the hosted service's execution.
		/// </summary>
		void Cancel();

		/// <summary>
		/// Starts the hosted service's execution.
		/// </summary>
		void Start();
	}
}