using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Euclid.Common.ServiceHost
{
	public class MultitaskingServiceHost : IServiceHost
	{
		private readonly TimeSpan _shutdownTimeout;
		private readonly IDictionary<Guid, Task> _taskMap;
		private readonly IDictionary<Guid, CancellationTokenSource> _taskTokenSources;

		public MultitaskingServiceHost()
		{
			_taskMap = new Dictionary<Guid, Task>();
			_taskTokenSources = new Dictionary<Guid, CancellationTokenSource>();
			_shutdownTimeout = TimeSpan.Parse("00:00:10");

			Services = new Dictionary<Guid, IHostedService>();
		}

		public IDictionary<Guid, IHostedService> Services { get; private set; }
		public ServiceHostState State { get; private set; }

		public HostedServiceState GetState(Guid id)
		{
			checkForHostedService(id);

			return Services[id].State;
		}

		public Guid Install(IHostedService service)
		{
			var serviceId = Guid.NewGuid();

			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;

			var task = createTask(service, cancellationToken);

			_taskMap.Add(serviceId, task);

			_taskTokenSources.Add(serviceId, cancellationTokenSource);

			Services.Add(serviceId, service);

			return serviceId;
		}

		public void Start(Guid id)
		{
			checkForHostedService(id);

			State = ServiceHostState.Starting;

			_taskMap[id].Start();

			State = ServiceHostState.Started;
		}

		public void StartAll()
		{
			State = ServiceHostState.Starting;

			foreach (var task in _taskMap.Select(taskMapEntry => taskMapEntry.Value))
			{
				if(task.Status == TaskStatus.WaitingToRun || task.Status == TaskStatus.Created)
				{
					task.Start();
				}
			}

			State = ServiceHostState.Started;
		}

		public void Stop(Guid id)
		{
			checkForHostedService(id);

			State = ServiceHostState.Stopping;

			_taskTokenSources[id].Cancel();

			State = ServiceHostState.Stopped;
		}

		public void StopAll()
		{
			State = ServiceHostState.Stopping;

			foreach (var tokenSource in _taskTokenSources.Values)
			{
				tokenSource.Cancel();
			}

			Task.WaitAll(_taskMap.Values.ToArray(), _shutdownTimeout);

			State = ServiceHostState.Stopped;
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
			cancellationToken.Register(service.Stop);

			return new Task(service.Start, cancellationToken, TaskCreationOptions.LongRunning);
		}
	}
}