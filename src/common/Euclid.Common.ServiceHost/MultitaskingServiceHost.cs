using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Euclid.Common.Logging;

namespace Euclid.Common.ServiceHost
{
	/// <summary>
	/// 	A basic concurrent service host using the Task Parallel Library.
	/// </summary>
	public class MultitaskingServiceHost : IServiceHost, ILoggingSource
	{
		private readonly IList<Exception> _serviceExceptions;

		private readonly TimeSpan _shutdownTimeout;

		private readonly IDictionary<Guid, Task> _taskMap;

		private readonly IDictionary<Guid, CancellationTokenSource> _taskTokenSources;

		public MultitaskingServiceHost()
		{
			this._taskMap = new Dictionary<Guid, Task>();
			this._taskTokenSources = new Dictionary<Guid, CancellationTokenSource>();
			this._shutdownTimeout = TimeSpan.Parse("00:00:10");
			this._serviceExceptions = new List<Exception>();

			this.Services = new Dictionary<Guid, IHostedService>();
		}

		public IDictionary<Guid, IHostedService> Services { get; private set; }

		public ServiceHostState State { get; private set; }

		public void Cancel(Guid id)
		{
			this.checkForHostedService(id);

			this.WriteInfoMessage(string.Format("Cancelling hosted service {0}, identifier {1}.", this.Services[id].Name, id));

			this.State = ServiceHostState.Stopping;

			this._taskTokenSources[id].Cancel();

			this._taskMap[id].Wait();

			this.State = ServiceHostState.Stopped;
		}

		public void CancelAll()
		{
			this.State = ServiceHostState.Stopping;

			this.WriteInfoMessage(string.Format("Cancelling {0} hosted services.", this.Services.Count));

			foreach (var tokenSource in this._taskTokenSources.Values)
			{
				tokenSource.Cancel();
			}

			try
			{
				Task.WaitAll(this._taskMap.Values.ToArray(), this._shutdownTimeout);
			}
			catch (AggregateException e)
			{
				foreach (var innerException in e.InnerExceptions)
				{
					this._serviceExceptions.Add(innerException);
				}
			}

			this.State = ServiceHostState.Stopped;
		}

		public IList<Exception> GetExceptionsThrownByHostedServices()
		{
			foreach (var task in this._taskMap)
			{
				if (task.Value.Exception != null)
				{
					foreach (var innerException in task.Value.Exception.InnerExceptions)
					{
						this._serviceExceptions.Add(innerException);
					}
				}
			}

			return this._serviceExceptions;
		}

		public HostedServiceState GetState(Guid id)
		{
			this.checkForHostedService(id);

			return this.Services[id].State;
		}

		public Guid Install(IHostedService service)
		{
			this.WriteDebugMessage(string.Format("Installing hosted service {0}({1}).", service.GetType().Name, service.Name));

			var serviceId = Guid.NewGuid();

			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;

			var task = this.createTask(service, cancellationToken);

			this._taskMap.Add(serviceId, task);

			this._taskTokenSources.Add(serviceId, cancellationTokenSource);

			this.Services.Add(serviceId, service);

			this.WriteInfoMessage(string.Format("Installed hosted service {0}.", service.Name));

			return serviceId;
		}

		public void Start(Guid id)
		{
			this.checkForHostedService(id);

			this.State = ServiceHostState.Starting;

			this._taskMap[id].Start();

			this.State = ServiceHostState.Started;

			this.WriteInfoMessage(string.Format("Started hosted service {0}, identifier {1}.", this.Services[id].Name, id));
		}

		public void StartAll()
		{
			this.State = ServiceHostState.Starting;

			this.WriteDebugMessage(string.Format("Starting service host with {0} hosted services.", this.Services.Count));

			foreach (var task in this._taskMap.Select(taskMapEntry => taskMapEntry.Value))
			{
				if (task.Status == TaskStatus.WaitingToRun || task.Status == TaskStatus.Created)
				{
					task.Start();
				}
			}

			this.State = ServiceHostState.Started;

			this.WriteInfoMessage(string.Format("Started service host with {0} hosted services.", this.Services.Count));
		}

		private void checkForHostedService(Guid id)
		{
			if (!this._taskMap.ContainsKey(id))
			{
				throw new HostedServiceNotFoundException(id);
			}
		}

		private Task createTask(IHostedService service, CancellationToken cancellationToken)
		{
			cancellationToken.Register(service.Cancel);

			return new Task(service.Start, cancellationToken, TaskCreationOptions.LongRunning);
		}
	}
}