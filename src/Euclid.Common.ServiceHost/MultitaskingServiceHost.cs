using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Euclid.Common.Hosting;

namespace Euclid.Common.ServiceHost
{
	public class MultitaskingServiceHost : IServiceHost
	{
		private readonly IDictionary<Guid, CancellationTokenSource> _taskTokenSources;
		private readonly IDictionary<Guid, List<Task>> _tasks;

		public MultitaskingServiceHost()
		{
			_tasks = new Dictionary<Guid, List<Task>>();
			_taskTokenSources = new Dictionary<Guid, CancellationTokenSource>();
			Services = new Dictionary<Guid, IHostedService>();
			Scale = 1;
		}

		public int Scale { get; private set; }
		public IDictionary<Guid, IHostedService> Services { get; private set; }
		public ServiceHostState State { get; private set; }

		public HostedServiceState GetState(Guid id)
		{
			checkForHostedService(id);

			return _taskTokenSources[id].IsCancellationRequested ? HostedServiceState.Started : HostedServiceState.Stopped;
		}

		public Guid Install(IHostedService service)
		{
			var serviceId = Guid.NewGuid();

			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;

			var task = createTask(service, cancellationToken);

			_tasks.Add(serviceId, new List<Task> {task});
			_taskTokenSources.Add(serviceId, cancellationTokenSource);
			Services.Add(serviceId, service);

			return serviceId;
		}

		public void ScaleDown(Guid id)
		{
			checkForHostedService(id);

			Scale--;
		}

		public void ScaleUp(Guid id)
		{
			checkForHostedService(id);

			var service = Services[id];

			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;

			var task = createTask(service, cancellationToken);

			task.Start();

			var tasks = _tasks[id];

			tasks.Add(task);

			Scale++;
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

		public void Stop(Guid id)
		{
			checkForHostedService(id);

			State = ServiceHostState.Stopping;

			_taskTokenSources[id].Cancel();

			State = ServiceHostState.Stopped;
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
	}
}