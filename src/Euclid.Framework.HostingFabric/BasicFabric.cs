using System;
using System.Collections.Generic;
using Euclid.Common.HostingFabric;
using Euclid.Common.ServiceHost;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Framework.HostingFabric
{
	public class BasicFabric : IFabricRuntime
	{
		protected IList<Type> ConfiguredHostedServices;
		protected IServiceLocator Container;
		protected IFabricRuntimeSettings CurrentSettings;
		private IServiceHost _serviceHost;

		public BasicFabric(IServiceLocator container)
		{
			Container = container;
			State = FabricRuntimeState.Stopped;
			ConfiguredHostedServices = new List<Type>();
		}

		public FabricRuntimeState State { get; protected set; }

		public virtual void Initialize(IFabricRuntimeSettings settings)
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

			CurrentSettings = settings;
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
			var hostedServices = new List<IHostedService>();

			foreach (var hostedServiceType in CurrentSettings.HostedServices.Value)
			{
				try
				{
					hostedServices.Add((IHostedService)Container.GetInstance(hostedServiceType));

					ConfiguredHostedServices.Add(hostedServiceType);
				}
				catch (ActivationException e)
				{
					throw new HostedServiceNotResolvableException
						(string.Format("Unable to resolve hosted service of type {0} from container.", CurrentSettings.ServiceHost.Value), e);
				}
			}

			foreach (var hostedService in hostedServices)
			{
				_serviceHost.Install(hostedService);
			}

			_serviceHost.StartAll();

			State = FabricRuntimeState.Started;
		}
	}
}