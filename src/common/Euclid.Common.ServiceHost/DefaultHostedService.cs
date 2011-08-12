using Euclid.Common.Logging;

namespace Euclid.Common.ServiceHost
{
	public abstract class DefaultHostedService : IHostedService, ILoggingSource
	{
		public string Name
		{
			get { return GetType().Name; }
		}

		public HostedServiceState State { get; protected set; }

		public void Cancel()
		{
			State = HostedServiceState.Stopping;
			OnStop();
			State = HostedServiceState.Stopped;

			this.WriteInfoMessage("{0} stopped", GetType().FullName);
		}

		public void Start()
		{
			State = HostedServiceState.Started;
			OnStart();

			this.WriteInfoMessage("{0} started", GetType().FullName);
		}

		protected abstract void OnStart();
		protected abstract void OnStop();
	}
}