using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Euclid.Framework.HostingFabric
{
	public class ThreadedServiceHost : IServiceHost
	{
		private readonly IDictionary<Guid, IHostedService> _services;
		private readonly IDictionary<Guid, Task> _tasks;

		public ThreadedServiceHost()
		{
			_tasks = new Dictionary<Guid, Task>();
			_services = new Dictionary<Guid, IHostedService>();
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
			var newId = Guid.NewGuid();

			var newThread = new Task(service.Start);

			_tasks.Add(newId, newThread);
			_services.Add(newId, service);

			return newId;
		}

		public void Start(Guid id)
		{
			checkForHostedService(id);

			_tasks[id].Start();
		}

		public void Pause(Guid id)
		{
			checkForHostedService(id);
		}

		public void Terminate(Guid id)
		{
			checkForHostedService(id);
		}

		public void ScaleUp(Guid id)
		{
			checkForHostedService(id);
		}

		public void ScaleDown(Guid id)
		{
			checkForHostedService(id);
		}

		public ServiceHostState GetState(Guid id)
		{
			checkForHostedService(id);
		}
	}
}