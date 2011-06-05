using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Euclid.Framework.HostingFabric.ServiceHosts
{
	public class MultitaskingServiceHost : IServiceHost
	{
		private readonly IDictionary<Guid, IHostedService> _services;
		private readonly IDictionary<Guid, CancellationTokenSource> _taskTokenSources;
		private readonly IDictionary<Guid, List<Task>> _tasks;

		public MultitaskingServiceHost()
		{
			_tasks = new Dictionary<Guid, List<Task>>();
			_services = new Dictionary<Guid, IHostedService>();
			_taskTokenSources = new Dictionary<Guid, CancellationTokenSource>();
		}

		public void GetInstanceState(Guid id)
		{
			checkForHostedService(id);
		}

		private void checkForHostedService(Guid id)
		{
			if (!_tasks.ContainsKey(id))
			{
				throw new HostedServiceNotFoundException(id);
			}
		}

		private Task createTask(IHostedService service, CancellationToken cancellationToken)
		{
			return new Task(service.Start, cancellationToken, TaskCreationOptions.LongRunning);
		}

		public Guid Install(IHostedService service)
		{
			var serviceId = Guid.NewGuid();

			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;

			var task = createTask(service, cancellationToken);

			_tasks.Add(serviceId, new List<Task> {task});
			_taskTokenSources.Add(serviceId, cancellationTokenSource);
			_services.Add(serviceId, service);

			return serviceId;
		}

		public void Start(Guid id)
		{
			checkForHostedService(id);

			State = ServiceHostState.Starting;

			foreach (var task in _tasks[id])
			{
				task.Start();
			}

			State = ServiceHostState.Started;
		}

		public void Pause(Guid id)
		{
			checkForHostedService(id);

			State = ServiceHostState.Pausing;

			State = ServiceHostState.Paused;
		}

		public void Terminate(Guid id)
		{
			checkForHostedService(id);

			State = ServiceHostState.Stopping;

			_taskTokenSources[id].Cancel();

			State = ServiceHostState.Stopped;
		}

		public void ScaleUp(Guid id)
		{
			checkForHostedService(id);

			var service = _services[id];

			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;

			var task = createTask(service, cancellationToken);

			task.Start();

			var tasks = _tasks[id];

			tasks.Add(task);
		}

		public void ScaleDown(Guid id)
		{
			checkForHostedService(id);

		}

		public ServiceHostState State { get; private set; }

		public HostedServiceState GetState(Guid id)
		{
			checkForHostedService(id);

			return _taskTokenSources[id].IsCancellationRequested ? HostedServiceState.Started : HostedServiceState.Stopped;
		}
	}
}