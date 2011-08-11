using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Agent.Extensions;
using Euclid.Common.HostingFabric;
using Euclid.Common.Messaging;
using Euclid.Common.ServiceHost;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata.Attributes;
using Microsoft.Practices.ServiceLocation;

namespace Euclid.Framework.HostingFabric
{
	public class BasicFabric : IFabricRuntime
	{
		protected IList<Type> ConfiguredHostedServices;
		protected IWindsorContainer Container;
		protected IFabricRuntimeSettings CurrentSettings;
		private IServiceHost _serviceHost;
		protected ICompositeApp Composite;

		public BasicFabric(IWindsorContainer container)
		{
			Container = container;
			State = FabricRuntimeState.Stopped;
			ConfiguredHostedServices = new List<Type>();
		}

		public FabricRuntimeState State { get; protected set; }

		public virtual IList<Exception> GetExceptionsThrownByHostedServices()
		{
			return _serviceHost.GetExceptionsThrownByHostedServices();
		}

		public virtual IFabricRuntimeStatistics GetStatistics()
		{
			return new DefaultRuntimeStatistics
				(_serviceHost.GetExceptionsThrownByHostedServices(), ConfiguredHostedServices, _serviceHost.GetType(), State, CurrentSettings);
		}

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
				_serviceHost = (IServiceHost) Container.Resolve(settings.ServiceHost.Value);
			}
			catch (ActivationException e)
			{
				throw new ServiceHostNotResolvableException
					(string.Format("Unable to resolve service host of type {0} from container.", settings.ServiceHost.Value), e);
			}

			CurrentSettings = settings;
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
					hostedServices.Add((IHostedService)Container.Resolve(hostedServiceType));

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

		public void InstallComposite(ICompositeApp composite)
		{
			if (Composite != null)
			{
				throw new Exception("A composite has already been installed");
			}

			if (composite.State != CompositeApplicationState.Configured)
			{
				throw new Exception("Only configured composites can be installed in the fabric.");
			}

			Composite = composite;

			foreach (var agent in Composite.Agents)
			{
				var processorAttribute = agent.AgentAssembly.GetAttributeValue<LocationOfProcessorsAttribute>();

				Container.Register
					(AllTypes.FromAssembly(agent.AgentAssembly)
					 	.Where(Component.IsInNamespace(processorAttribute.Namespace))
					 	.BasedOn(typeof (ICommandProcessor))
					 	.WithService.AllInterfaces().WithService.Self());

				var registry = Container.Resolve<ICommandRegistry>();

				var dispatcher = new CommandDispatcher(new WindsorServiceLocator(Container), registry);

				var dispatcherSettings = new MessageDispatcherSettings();

				dispatcherSettings.InputChannel.WithDefault(CurrentSettings.InputChannel.Value);
				dispatcherSettings.InvalidChannel.WithDefault(CurrentSettings.ErrorChannel.Value);

				var processors = Container.ResolveAll(typeof(ICommandProcessor));

				foreach (var processor in processors)
				{
					dispatcherSettings.MessageProcessorTypes.Add(processor.GetType());
				}

				dispatcher.Configure(dispatcherSettings);

				var commandHost = new CommandHost(new ICommandDispatcher[] {dispatcher});

				Container.Register(Component.For<IHostedService>().Instance(commandHost).Forward<CommandHost>());
			}
		}
	}
}