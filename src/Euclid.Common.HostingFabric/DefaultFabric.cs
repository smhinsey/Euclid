using System;
using System.Collections.Generic;
using Euclid.Common.ServiceHost;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Common.HostingFabric
{
	public class DefaultFabric : IFabricRuntime
	{
		protected IList<Type> ConfiguredHostedServices;
		protected IServiceLocator Container;
		protected IFabricRuntimeSettings CurrentSettings;
		private IServiceHost _serviceHost;

		public DefaultFabric(IServiceLocator container)
		{
			Container = container;
			State = FabricRuntimeState.Stopped;
			ConfiguredHostedServices = new List<Type>();
		}

		public FabricRuntimeState State { get; protected set; }

		public virtual void Configure(IFabricRuntimeSettings settings)
		{
			if (settings.ServiceHost.Value == null)
			{
				throw new NoServiceHostConfiguredException("You must configure a service host.");
			}

			if (settings.HostedServices.Value == null || settings.HostedServices.Value.Count == 0)
			{
				throw new NoHostedServicesConfiguredException("You must configure hosted services.");
			}

			CurrentSettings = settings;

			try
			{
				_serviceHost = (IServiceHost) Container.GetInstance(settings.ServiceHost.Value);
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
					hostedServices.Add((IHostedService) Container.GetInstance(hostedServiceType));

					ConfiguredHostedServices.Add(hostedServiceType);
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

		public virtual IList<Exception> GetExceptionsThrownByHostedServices()
		{
			return _serviceHost.GetExceptionsThrownByHostedServices();
		}

		public virtual IFabricRuntimeStatistics GetStatistics()
		{
			return new DefaultRuntimeStatistics
				(_serviceHost.GetExceptionsThrownByHostedServices(), ConfiguredHostedServices, _serviceHost.GetType(), State, CurrentSettings);
		}

		public virtual void Shutdown()
		{
			State = FabricRuntimeState.Stoppping;

			_serviceHost.CancelAll();

			State = FabricRuntimeState.Stopped;
		}

		public virtual void Start()
		{
			_serviceHost.StartAll();

			State = FabricRuntimeState.Started;
		}
	}
}