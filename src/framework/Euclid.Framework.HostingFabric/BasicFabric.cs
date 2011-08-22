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
	public class BasicFabric : IFabricRuntime, ILoggingSource
	{
		protected ICompositeApp Composite;

		protected IList<Type> ConfiguredHostedServices;

		protected IWindsorContainer Container;

		protected IFabricRuntimeSettings CurrentSettings;

		private IServiceHost _serviceHost;

		public BasicFabric(IWindsorContainer container)
		{
			this.Container = container;
			this.State = FabricRuntimeState.Stopped;
			this.ConfiguredHostedServices = new List<Type>();
		}

		public FabricRuntimeState State { get; protected set; }

		public virtual IList<Exception> GetExceptionsThrownByHostedServices()
		{
			return this._serviceHost.GetExceptionsThrownByHostedServices();
		}

		public virtual IFabricRuntimeStatistics GetStatistics()
		{
			return new DefaultRuntimeStatistics(
				this._serviceHost.GetExceptionsThrownByHostedServices(), 
				this.ConfiguredHostedServices, 
				this._serviceHost.GetType(), 
				this.State, 
				this.CurrentSettings);
		}

		public virtual void Initialize(IFabricRuntimeSettings settings)
		{
			this.WriteDebugMessage(string.Format("Initializing {0}", this.GetType().Name));

			if (settings.ServiceHost.Value == null)
			{
				throw new NoServiceHostConfiguredException("You must configure a service host.");
			}

			if (settings.HostedServices.Value == null || settings.HostedServices.Value.Count == 0)
			{
				throw new NoHostedServicesConfiguredException("You must configure hosted services.");
			}

			this.CurrentSettings = settings;

			try
			{
				this._serviceHost = (IServiceHost)this.Container.Resolve(settings.ServiceHost.Value);
			}
			catch (ComponentNotFoundException e)
			{
				throw new ServiceHostNotResolvableException(
					string.Format("Unable to resolve service host of type {0} from container.", settings.ServiceHost.Value), e);
			}

			this.CurrentSettings = settings;

			this.WriteInfoMessage(string.Format("Initialized {0}.", this.GetType().Name));
		}

		public void InstallComposite(ICompositeApp composite)
		{
			this.WriteDebugMessage(string.Format("Installing composite {0}.", composite.GetType().FullName));

			if (this.Composite != null)
			{
				throw new CompositeAlreadyInstalledException();
			}

			if (composite.State != CompositeApplicationState.Configured)
			{
				throw new CompositeNotConfiguredException();
			}

			this.Composite = composite;

			this.Container.Register(
				Component.For(typeof(ISimpleRepository<>)).ImplementedBy(typeof(NhSimpleRepository<>)).LifeStyle.Transient);

			this.extractProcessorsFromAgents();

			this.WriteInfoMessage(string.Format("Installed composite {0}.", composite.GetType().Name));
		}

		public virtual void Shutdown()
		{
			this.WriteDebugMessage(string.Format("Shutting down {0}.", this.GetType().Name));

			this.State = FabricRuntimeState.Stoppping;

			this._serviceHost.CancelAll();

			this.State = FabricRuntimeState.Stopped;

			this.WriteInfoMessage(string.Format("Shut down {0}.", this.GetType().Name));
		}

		public virtual void Start()
		{
			this.WriteDebugMessage(string.Format("Starting {0}.", this.GetType().Name));

			var hostedServices = new List<IHostedService>();

			foreach (var hostedServiceType in this.CurrentSettings.HostedServices.Value)
			{
				try
				{
					hostedServices.Add((IHostedService)this.Container.Resolve(hostedServiceType));

					this.ConfiguredHostedServices.Add(hostedServiceType);
				}
				catch (ComponentNotFoundException e)
				{
					throw new HostedServiceNotResolvableException(
						string.Format(
							"Unable to resolve hosted service of type {0} from container.", this.CurrentSettings.ServiceHost.Value), 
						e);
				}
			}

			foreach (var hostedService in hostedServices)
			{
				this._serviceHost.Install(hostedService);
			}

			this._serviceHost.StartAll();

			this.State = FabricRuntimeState.Started;

			this.WriteInfoMessage(string.Format("Started {0}.", this.GetType().Name));
		}

		private void extractProcessorsFromAgents()
		{
			foreach (var agent in this.Composite.Agents)
			{
				var processorAttribute = agent.AgentAssembly.GetAttributeValue<LocationOfProcessorsAttribute>();

				// SELF the Where call below changes the meaning of the rest of the registration so it had to be removed
				this.Container.Register(
					AllTypes.FromAssembly(agent.AgentAssembly)
            
						
						// .Where(Component.IsInNamespace(processorAttribute.Namespace))
						.BasedOn(typeof(ICommandProcessor)).Configure(c => c.LifeStyle.Transient).WithService.AllInterfaces().WithService.
						Self());

				var registry = this.Container.Resolve<ICommandRegistry>();

				var dispatcher = new CommandDispatcher(new WindsorServiceLocator(this.Container), registry);

				var dispatcherSettings = new MessageDispatcherSettings();

				dispatcherSettings.InputChannel.WithDefault(this.CurrentSettings.InputChannel.Value);
				dispatcherSettings.InvalidChannel.WithDefault(this.CurrentSettings.ErrorChannel.Value);

				var processors = this.Container.ResolveAll(typeof(ICommandProcessor));

				foreach (var processor in processors)
				{
					dispatcherSettings.MessageProcessorTypes.Add(processor.GetType());
				}

				dispatcher.Configure(dispatcherSettings);

				var commandHost = new CommandHost(new ICommandDispatcher[] { dispatcher });

				this.Container.Register(
					Component.For<IHostedService>().Instance(commandHost).Forward<CommandHost>().LifeStyle.Transient);
			}
		}
	}
}