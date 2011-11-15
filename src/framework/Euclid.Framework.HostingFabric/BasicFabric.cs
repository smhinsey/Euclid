using System;
using System.Collections.Generic;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Euclid.Common.Logging;
using Euclid.Common.Messaging;
using Euclid.Common.ServiceHost;
using Euclid.Common.Storage.Model;
using Euclid.Common.Storage.NHibernate;
using Euclid.Composites;
using Euclid.Framework.Agent;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.HostingFabric
{
	public class BasicFabric : ILoggingSource, IFabricRuntime
	{
		protected ICompositeApp Composite;

		protected IList<Type> ConfiguredHostedServices;

		protected IWindsorContainer Container;

		protected IFabricRuntimeSettings CurrentSettings;

		private IServiceHost _serviceHost;

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
			return new DefaultRuntimeStatistics(
				_serviceHost.GetExceptionsThrownByHostedServices(),
				ConfiguredHostedServices,
				_serviceHost.GetType(),
				State,
				CurrentSettings);
		}

		public virtual void Initialize(IFabricRuntimeSettings settings)
		{
			this.WriteDebugMessage(string.Format("Initializing {0}", GetType().Name));

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
				_serviceHost = (IServiceHost)Container.Resolve(settings.ServiceHost.Value);
			}
			catch (ComponentNotFoundException e)
			{
				throw new ServiceHostNotResolvableException(
					string.Format("Unable to resolve service host of type {0} from container.", settings.ServiceHost.Value), e);
			}

			CurrentSettings = settings;

			this.WriteInfoMessage(string.Format("Initialized {0}.", GetType().Name));
		}

		public void InstallComposite(ICompositeApp composite)
		{
			this.WriteDebugMessage(string.Format("Installing composite {0}.", composite.GetType().FullName));

			if (Composite != null)
			{
				throw new CompositeAlreadyInstalledException();
			}

			if (composite.State != CompositeApplicationState.Configured)
			{
				throw new CompositeNotConfiguredException();
			}

			Composite = composite;

			Container.Register(
				Component.For(typeof(ISimpleRepository<>)).ImplementedBy(typeof(NhSimpleRepository<>)).LifeStyle.Transient);

			extractProcessorsFromAgents();

			this.WriteInfoMessage(string.Format("Installed composite {0}.", composite.GetType().Name));
		}

		public virtual void Shutdown()
		{
			this.WriteDebugMessage(string.Format("Shutting down {0}.", GetType().Name));

			State = FabricRuntimeState.Stoppping;

			_serviceHost.CancelAll();

			State = FabricRuntimeState.Stopped;

			this.WriteInfoMessage(string.Format("Shut down {0}.", GetType().Name));
		}

		public virtual void Start()
		{
			this.WriteDebugMessage(string.Format("Starting {0}.", GetType().Name));

			var hostedServices = new List<IHostedService>();

			foreach (var hostedServiceType in CurrentSettings.HostedServices.Value)
			{
				try
				{
					hostedServices.Add((IHostedService)Container.Resolve(hostedServiceType));

					ConfiguredHostedServices.Add(hostedServiceType);
				}
				catch (ComponentNotFoundException e)
				{
					throw new HostedServiceNotResolvableException(
						string.Format("Unable to resolve hosted service of type {0} from container.", CurrentSettings.ServiceHost.Value),
						e);
				}
			}

			foreach (var hostedService in hostedServices)
			{
				_serviceHost.Install(hostedService);
			}

			_serviceHost.StartAll();

			State = FabricRuntimeState.Started;

			this.WriteInfoMessage(string.Format("Started {0}.", GetType().Name));
		}

		private void extractProcessorsFromAgents()
		{
			foreach (var agent in Composite.Agents)
			{
				var processorAttribute = agent.AgentAssembly.GetAttributeValue<LocationOfProcessorsAttribute>();

				// SELF the Where call below changes the meaning of the rest of the registration so it had to be removed
				Container.Register(
					AllTypes.FromAssembly(agent.AgentAssembly)
						// .Where(Component.IsInNamespace(processorAttribute.Namespace))
						.BasedOn(typeof(ICommandProcessor)).Configure(c => c.LifeStyle.Transient).WithService.AllInterfaces().WithService.
						Self());

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

				var commandHost = new CommandHost(new ICommandDispatcher[] { dispatcher });

				Container.Register(Component.For<IHostedService>().Instance(commandHost).Forward<CommandHost>().LifeStyle.Transient);
			}
		}
	}
}