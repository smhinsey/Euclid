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
			_taskMap = new Dictionary<Guid, Task>();
			_taskTokenSources = new Dictionary<Guid, CancellationTokenSource>();
			_shutdownTimeout = TimeSpan.Parse("00:00:10");
			_serviceExceptions = new List<Exception>();

			Services = new Dictionary<Guid, IHostedService>();
		}

		public IDictionary<Guid, IHostedService> Services { get; private set; }

		public ServiceHostState State { get; private set; }

		public void Cancel(Guid id)
		{
			checkForHostedService(id);

			this.WriteInfoMessage(string.Format("Cancelling hosted service {0}, identifier {1}.", Services[id].Name, id));

			State = ServiceHostState.Stopping;

			_taskTokenSources[id].Cancel();

			_taskMap[id].Wait();

			State = ServiceHostState.Stopped;
		}

		public void CancelAll()
		{
			State = ServiceHostState.Stopping;

			this.WriteInfoMessage(string.Format("Cancelling {0} hosted services.", Services.Count));

			foreach (var tokenSource in _taskTokenSources.Values)
			{
				tokenSource.Cancel();
			}

			try
			{
				Task.WaitAll(_taskMap.Values.ToArray(), _shutdownTimeout);
			}
			catch (AggregateException e)
			{
				foreach (var innerException in e.InnerExceptions)
				{
					_serviceExceptions.Add(innerException);
				}
			}

			State = ServiceHostState.Stopped;
		}

		public IList<Exception> GetExceptionsThrownByHostedServices()
		{
			foreach (var task in _taskMap)
			{
				if (task.Value.Exception != null)
				{
					foreach (var innerException in task.Value.Exception.InnerExceptions)
					{
						_serviceExceptions.Add(innerException);
					}
				}
			}

			return _serviceExceptions;
		}

		public HostedServiceState GetState(Guid id)
		{
			checkForHostedService(id);

			return Services[id].State;
		}

		public Guid Install(IHostedService service)
		{
			this.WriteDebugMessage(string.Format("Installing hosted service {0}({1}).", service.GetType().Name, service.Name));

			var serviceId = Guid.NewGuid();

			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;

			var task = createTask(service, cancellationToken);

			_taskMap.Add(serviceId, task);

			_taskTokenSources.Add(serviceId, cancellationTokenSource);

			Services.Add(serviceId, service);

			this.WriteInfoMessage(string.Format("Installed hosted service {0}.", service.Name));

			return serviceId;
		}

		public void Start(Guid id)
		{
			checkForHostedService(id);

			State = ServiceHostState.Starting;

			_taskMap[id].Start();

			State = ServiceHostState.Started;

			this.WriteInfoMessage(string.Format("Started hosted service {0}, identifier {1}.", Services[id].Name, id));
		}

		public void StartAll()
		{
			State = ServiceHostState.Starting;

			this.WriteDebugMessage(string.Format("Starting service host with {0} hosted services.", Services.Count));

			foreach (var task in _taskMap.Select(taskMapEntry => taskMapEntry.Value))
			{
				if (task.Status == TaskStatus.WaitingToRun || task.Status == TaskStatus.Created)
				{
					task.Start();
				}
			}

			State = ServiceHostState.Started;

			this.WriteInfoMessage(string.Format("Started service host with {0} hosted services.", Services.Count));
		}

		private void checkForHostedService(Guid id)
		{
			if (!_taskMap.ContainsKey(id))
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