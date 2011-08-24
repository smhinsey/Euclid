namespace Euclid.Common.ServiceHost
{
	/// <summary>
	/// 	Indicates the runtime state of a hosted service.
	/// </summary>
	public enum HostedServiceState
	{
		/// <summary>
		/// 	The hosted service has not be started.
		/// </summary>
		Unspecified, 

		/// <summary>
		/// 	The hosted service is started and running without error.
		/// </summary>
		Started, 

		/// <summary>
		/// 	The hosted service is stopping.
		/// </summary>
		Stopping, 

		/// <summary>
		/// 	The hosted service is stopped.
		/// </summary>
		Stopped
	}
}
