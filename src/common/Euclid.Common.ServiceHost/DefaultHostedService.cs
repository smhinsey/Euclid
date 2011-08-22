using Euclid.Common.Logging;

namespace Euclid.Common.ServiceHost
{
	public abstract class DefaultHostedService : IHostedService, ILoggingSource
	{
		public string Name
		{
			get
			{
				return this.GetType().Name;
			}
		}

		public HostedServiceState State { get; protected set; }

		public void Cancel()
		{
			this.WriteDebugMessage(string.Format("Cancelling {0}.", this.GetType().FullName));

			this.State = HostedServiceState.Stopping;
			this.OnStop();
			this.State = HostedServiceState.Stopped;

			this.WriteInfoMessage("Cancelled {0}.", this.GetType().Name);
		}

		public void Start()
		{
			this.WriteDebugMessage(string.Format("Starting {0}.", this.GetType().FullName));

			this.State = HostedServiceState.Started;
			this.OnStart();

			this.WriteInfoMessage("Started {0}.", this.GetType().Name);
		}

		protected abstract void OnStart();

		protected abstract void OnStop();
	}
}