using System;
using System.Collections.Generic;
using Castle.Windsor;
using Euclid.Common.ServiceHost;

namespace Euclid.Common.HostingFabric
{
	public class LocalMachineFabric : IFabricRuntime
	{
		private readonly IList<Type> _configuredHostedServices;
		private readonly IWindsorContainer _container;
		private IFabricRuntimeSettings _currentSettings;
		private IServiceHost _serviceHost;

		public LocalMachineFabric(IWindsorContainer container)
		{
			_container = container;
			State = FabricRuntimeState.Stopped;
			_configuredHostedServices = new List<Type>();
		}

		public FabricRuntimeState State { get; private set; }

		public void Configure(IFabricRuntimeSettings settings)
		{
			_currentSettings = settings;

			try
			{
				_serviceHost = (IServiceHost) _container.Resolve(settings.ServiceHost.Value);
			}
			catch (Exception e)
			{
				throw new RuntimeFabricConfigurationException
					(string.Format("Unable to resolve service host of type {0} from container.", settings.ServiceHost.Value), e);
			}

			var hostedServices = new List<IHostedService>();

			foreach (var hostedServiceType in settings.HostedServices.Value)
			{
				try
				{
					hostedServices.Add((IHostedService) _container.Resolve(hostedServiceType));

					_configuredHostedServices.Add(hostedServiceType);
				}
				catch (Exception e)
				{
					throw new RuntimeFabricConfigurationException
						(string.Format("Unable to resolve hosted service of type {0} from container.", settings.ServiceHost.Value), e);
				}
			}

			foreach (var hostedService in hostedServices)
			{
				_serviceHost.Install(hostedService);
			}
		}

		public IFabricRuntimeStatistics GetStatistics()
		{
			return new DefaultRuntimeStatistics(_configuredHostedServices, _serviceHost.GetType(), State, _currentSettings);
		}

		public void Shutdown()
		{
			_serviceHost.CancelAll();
		}

		public void Start()
		{
			_serviceHost.StartAll();
		}
	}
}