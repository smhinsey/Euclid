using System;
using System.Collections.Generic;
using System.Threading;

namespace Euclid.Framework.HostingFabric
{
	public class ThreadedServiceHost : IServiceHost
	{
		private readonly IDictionary<Guid, Thread> _threads;
		private readonly IDictionary<Guid, IHostedService> _services;

		public ThreadedServiceHost()
		{
			_threads = new Dictionary<Guid, Thread>();
			_services = new Dictionary<Guid, IHostedService>();
		}

		public Guid Install(IHostedService service)
		{
			var newId = Guid.NewGuid();

			var newThread = new Thread(service.Start);

			_threads.Add(newId, newThread);
			_services.Add(newId, service);

			return newId;
		}

		public void Start(Guid id)
		{
			if(!_threads.ContainsKey(id))
			{
				throw new HostedServiceNotFoundException(id);
			}
		}

		public void GetInstanceState(Guid id)
		{
			throw new NotImplementedException();
		}

		public void Pause(Guid id)
		{
			throw new NotImplementedException();
		}

		public void Terminate(Guid id)
		{
			throw new NotImplementedException();
		}

		public void ScaleUp(Guid id)
		{
			throw new NotImplementedException();
		}

		public void ScaleDown(Guid id)
		{
			throw new NotImplementedException();
		}

		public ServiceHostState GetState(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}