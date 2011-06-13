namespace Euclid.Common.ServiceHost
{
	public abstract class DefaultHostedService : IHostedService
	{
		public string Name
		{
			get { return GetType().Name; }
		}

		public HostedServiceState State { get; protected set; }

		public void Start()
		{
			State = HostedServiceState.Started;
			OnStart();
		}

		public void Cancel()
		{
			State = HostedServiceState.Stopping;
			OnStop();
			State = HostedServiceState.Stopped;
		}

		protected abstract void OnStart();
		protected abstract void OnStop();
	}
}