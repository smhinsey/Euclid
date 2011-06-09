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
		private readonly IDictionary<Guid, List<Task>> _taskMap;
		private readonly IDictionary<Guid, CancellationTokenSource> _taskTokenSources;

		public MultitaskingServiceHost(int initialScale = 1)
		{
			_taskMap = new Dictionary<Guid, List<Task>>();
			_taskTokenSources = new Dictionary<Guid, CancellationTokenSource>();
			_shutdownTimeout = TimeSpan.Parse("00:00:10");

			Services = new Dictionary<Guid, IHostedService>();
			Scale = initialScale;
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

			_taskMap.Add(serviceId, new List<Task> {task});

			_taskTokenSources.Add(serviceId, cancellationTokenSource);

			Services.Add(serviceId, service);

			return serviceId;
		}

		public void ScaleAllDown()
		{
			Scale--;
		}

		public void ScaleAllUp()
		{
			foreach (var serviceId in Services.Keys)
			{
				addTaskInstance(serviceId, Services[serviceId]);
			}

			Scale++;
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

			addTaskInstance(id, service);
		}

		public void Start(Guid id)
		{
			checkForHostedService(id);

			State = ServiceHostState.Starting;

			foreach (var task in _taskMap[id])
			{
				task.Start();
			}

			State = ServiceHostState.Started;
		}

		public void StartAll()
		{
			State = ServiceHostState.Starting;

			foreach (var taskMapEntry in _taskMap)
			{
				var tasks = taskMapEntry.Value;

				foreach (
					var task in tasks.Where(task => task.Status == TaskStatus.WaitingToRun || task.Status == TaskStatus.Created))
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

			var tasks = new List<Task>();

			foreach (var taskList in _taskMap.Values)
			{
				tasks.AddRange(taskList);
			}

			Task.WaitAll(tasks.ToArray(), _shutdownTimeout);

			State = ServiceHostState.Stopped;
		}

		private void addTaskInstance(Guid id, IHostedService service)
		{
			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;

			var task = createTask(service, cancellationToken);

			task.Start();

			var tasks = _taskMap[id];

			tasks.Add(task);

			Scale++;
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