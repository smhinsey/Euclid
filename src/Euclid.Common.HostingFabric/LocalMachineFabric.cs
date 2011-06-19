using System;
using System.Collections.Generic;
using Euclid.Common.ServiceHost;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Common.HostingFabric
{
	public class LocalMachineFabric : IFabricRuntime
	{
		private readonly IList<Type> _configuredHostedServices;
		private readonly IServiceLocator _container;
		private IFabricRuntimeSettings _currentSettings;
		private IServiceHost _serviceHost;

		public LocalMachineFabric(IServiceLocator container)
		{
			_container = container;
			State = FabricRuntimeState.Stopped;
			_configuredHostedServices = new List<Type>();
		}

		public FabricRuntimeState State { get; private set; }

		public void Configure(IFabricRuntimeSettings settings)
		{
			if (settings.ServiceHost.Value == null)
			{
				throw new NoServiceHostConfiguredException("You must configure a service host.");
			}

			if (settings.HostedServices.Value == null || settings.HostedServices.Value.Count == 0)
			{
				throw new NoHostedServicesConfiguredException("You must configure hosted services.");
			}

			_currentSettings = settings;

			try
			{
				_serviceHost = (IServiceHost) _container.GetInstance(settings.ServiceHost.Value);
			}
			catch (ActivationException e)
			{
				throw new ServiceHostNotResolvableException
					(string.Format("Unable to resolve service host of type {0} from container.", settings.ServiceHost.Value), e);
			}

			var hostedServices = new List<IHostedService>();

			foreach (var hostedServiceType in settings.HostedServices.Value)
			{
				try
				{
					hostedServices.Add((IHostedService) _container.GetInstance(hostedServiceType));

					_configuredHostedServices.Add(hostedServiceType);
				}
				catch (ActivationException e)
				{
					throw new HostedServiceNotResolvableException
						(string.Format("Unable to resolve hosted service of type {0} from container.", settings.ServiceHost.Value), e);
				}
			}

			foreach (var hostedService in hostedServices)
			{
				_serviceHost.Install(hostedService);
			}
		}

		public IList<Exception> GetExceptionsThrownByHostedServices()
		{
			return _serviceHost.GetExceptionsThrownByHostedServices();
		}

		public IFabricRuntimeStatistics GetStatistics()
		{
			return new DefaultRuntimeStatistics
				(_serviceHost.GetExceptionsThrownByHostedServices(), _configuredHostedServices, _serviceHost.GetType(), State, _currentSettings);
		}

		public void Shutdown()
		{
			State = FabricRuntimeState.Stoppping;

			_serviceHost.CancelAll();

			State = FabricRuntimeState.Stopped;
		}

		public void Start()
		{
			_serviceHost.StartAll();

			State = FabricRuntimeState.Started;
		}
	}
}