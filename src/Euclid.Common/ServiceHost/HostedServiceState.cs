namespace Euclid.Common.ServiceHost
{
	public enum HostedServiceState
	{
		Starting,
		Started,
		Stopping,
		Stopped,
		Pausing,
		Paused,
		Failed
	}
}