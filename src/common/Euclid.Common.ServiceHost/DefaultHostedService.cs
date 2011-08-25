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
			this.WriteDebugMessage(string.Format("Cancelling {0}.", GetType().FullName));

			State = HostedServiceState.Stopping;
			OnStop();
			State = HostedServiceState.Stopped;

			this.WriteInfoMessage("Cancelled {0}.", GetType().Name);
		}

		public void Start()
		{
			this.WriteDebugMessage(string.Format("Starting {0}.", GetType().FullName));

			State = HostedServiceState.Started;
			OnStart();

			this.WriteInfoMessage("Started {0}.", GetType().Name);
		}

		protected abstract void OnStart();

		protected abstract void OnStop();
	}
}
