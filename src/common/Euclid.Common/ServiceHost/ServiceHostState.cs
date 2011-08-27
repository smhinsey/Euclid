namespace Euclid.Common.ServiceHost
{
	/// <summary>
	/// 	Indicates the runtime state of a service host.
	/// </summary>
	public enum ServiceHostState
	{
		/// <summary>
		/// 	The service host is starting up.
		/// </summary>
		Starting,

		/// <summary>
		/// 	The service host is started and running without error.
		/// </summary>
		Started,

		/// <summary>
		/// 	The service host is stopping.
		/// </summary>
		Stopping,

		/// <summary>
		/// 	The service host is stopped.
		/// </summary>
		Stopped
	}
}