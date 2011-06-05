using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Euclid.Framework.HostingFabric
{
	public class TaskingServiceHost : IServiceHost
	{
		private readonly IDictionary<Guid, Type> _services;
		private readonly IDictionary<Guid, CancellationToken> _taskTokens;
		private readonly IDictionary<Guid, Task> _tasks;

		public TaskingServiceHost()
		{
			_tasks = new Dictionary<Guid, Task>();
			_services = new Dictionary<Guid, Type>();
			_taskTokens = new Dictionary<Guid, CancellationToken>();
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

		public Guid Install(IHostedService service)
		{
			var serviceId = Guid.NewGuid();

			var cancellationToken = new CancellationToken();

			var task = new Task(service.Start, cancellationToken, TaskCreationOptions.LongRunning);

			_tasks.Add(serviceId, task);
			_taskTokens.Add(serviceId, cancellationToken);
			_services.Add(serviceId, service.GetType());

			return serviceId;
		}

		public void Start(Guid id)
		{
			checkForHostedService(id);

			State = ServiceHostState.Starting;

			_tasks[id].Start();

			State = ServiceHostState.Started;
		}

		public void Pause(Guid id)
		{
			State = ServiceHostState.Pausing;

			checkForHostedService(id);

			State = ServiceHostState.Paused;
		}

		public void Terminate(Guid id)
		{
			State = ServiceHostState.Stopping;

			checkForHostedService(id);

			State = ServiceHostState.Stopped;
		}

		public void ScaleUp(Guid id)
		{
			checkForHostedService(id);
		}

		public void ScaleDown(Guid id)
		{
			checkForHostedService(id);
		}

		public ServiceHostState State { get; private set; }

		public HostedServiceState GetState(Guid id)
		{
			checkForHostedService(id);

			return _taskTokens[id].IsCancellationRequested ? HostedServiceState.Started : HostedServiceState.Stopped;
		}
	}
}