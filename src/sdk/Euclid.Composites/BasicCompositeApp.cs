using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Euclid.Agent;
using Euclid.Agent.Extensions;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.NHibernate;
using Euclid.Common.Storage.Record;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Conversion;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.NHibernate;
using Euclid.Framework.Models;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using IQuery = Euclid.Framework.Cqrs.IQuery;

namespace Euclid.Composites
{
	public class BasicCompositeApp : ICompositeApp
	{
		public BasicCompositeApp()
		{
			State = CompositeApplicationState.Uninitailized;
			Agents = new List<IAgentMetadata>();
			InputModelTransformers = new InputModelToCommandTransformerRegistry();
			Container = new WindsorContainer();
		}

		public BasicCompositeApp(IWindsorContainer container) : this()
		{
			Container = container;
		}

		public IList<IAgentMetadata> Agents { get; private set; }

		public CompositeApplicationState State { get; set; }

		protected IWindsorContainer Container { get; set; }

		protected IInputModelTransfomerRegistry InputModelTransformers { get; private set; }

		protected CompositeAppSettings Settings { get; set; }

		public virtual void Configure(CompositeAppSettings compositeAppSettings)
		{
			Container.Kernel.Resolver.AddSubResolver(new ArrayResolver(Container.Kernel));

			Container.Kernel.Resolver.AddSubResolver(new ListResolver(Container.Kernel));

			RegisterConfiguredTypes(compositeAppSettings);

			Container.Register(Component
			                   	.For<IInputModelTransfomerRegistry>()
			                   	.Instance(InputModelTransformers)
			                   	.LifeStyle.Singleton);

			Settings = compositeAppSettings;
			State = CompositeApplicationState.Configured;
		}

		public void AddAgent(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			if (!assembly.ContainsAgent())
			{
				throw new AssemblyNotAgentException(assembly);
			}

			// enumerate queries, commands & read models
			var agent = assembly.GetAgentMetadata();

			Agents.Add(agent);

			// SELF the Where call below changes the meaning of the rest of the registration so it had to be removed

			Container.Register
				(AllTypes.FromAssembly(agent.AgentAssembly)
					//.Where(Component.IsInNamespace(agent.Queries.Namespace))
				 	.BasedOn(typeof (IQuery))
					.WithService.Self()
				 	.Configure(component => component.LifeStyle.Transient));
		}

		public void RegisterInputModel(IInputToCommandConverter converter)
		{
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}

			var commandMetadata = converter.CommandType.GetMetadata();

			var agent = Agents.Where(a => a.Commands.Namespace == commandMetadata.Namespace).FirstOrDefault();

			if (agent == null)
			{
				throw new AgentNotFoundException(commandMetadata.Namespace);
			}

			if (!agent.Commands.Where(x => x.Name == commandMetadata.Name).Any())
			{
				throw new CommandNotPresentInAgentException();
			}

			InputModelTransformers.Add(commandMetadata.Name, converter);
		}

		public void RegisterNh(IPersistenceConfigurer databaseConfiguration, bool buildSchema, bool isWeb)
		{
			var lifestyleType = (isWeb) ? LifestyleType.PerWebRequest : LifestyleType.Transient;

			Container.Register(
			                   Component.For<ISessionFactory>()
			                   	.UsingFactoryMethod(() =>
			                   	                    Fluently
			                   	                    	.Configure()
			                   	                    	.Database(databaseConfiguration)
			                   	                    	.Mappings(map => mapAllAssemblies(map))
			                   	                    	.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, buildSchema))
			                   	                    	.BuildSessionFactory()
			                   	).LifeStyle.Singleton);

			// jt: open session should be read-only
			Container.Register(
			                   Component.For<ISession>()
			                   	.UsingFactoryMethod(
			                   	                    () => Container.Resolve<ISessionFactory>().OpenSession()).LifeStyle.Is(lifestyleType)
				);
		}

		protected void RegisterConfiguredTypes(CompositeAppSettings compositeAppSettings)
		{
			Container.Register(Component.For<IPublisher>()
			                   	.ImplementedBy(compositeAppSettings.Publisher.Value)
													.LifeStyle.Transient);

			Container.Register(Component.For<IMessageChannel>()
			                   	.ImplementedBy(compositeAppSettings.MessageChannel.Value)
													.LifeStyle.Transient);

			Container.Register(Component.For<IRecordMapper<CommandPublicationRecord>>()
			                   	.ImplementedBy(compositeAppSettings.CommandPublicationRecordMapper.Value)
			                   	.LifeStyle.Transient);

			Container.Register(Component.For<IBlobStorage>()
			                   	.ImplementedBy(compositeAppSettings.BlobStorage.Value)
													.LifeStyle.Transient);

			Container.Register(Component.For<IMessageSerializer>()
			                   	.ImplementedBy(compositeAppSettings.MessageSerializer.Value)
													.LifeStyle.Transient);

			Container.Register(Component.For<IPublicationRegistry<IPublicationRecord>>()
			                   	.Forward<ICommandRegistry>()
			                   	.ImplementedBy(compositeAppSettings.PublicationRegistry.Value)
													.LifeStyle.Transient);
		}

		private MappingConfiguration mapAllAssemblies(MappingConfiguration mcfg)
		{
			var autoMapperConfiguration = new AutoMapperConfiguration();

			var assembliesToMap = new Dictionary<Assembly, Assembly>();

			if (Settings.CommandPublicationRecordMapper.Value == typeof (NhRecordMapper<CommandPublicationRecord>))
			{
				mcfg.AutoMappings.Add(AutoMap.AssemblyOf<CommandPublicationRecord>(autoMapperConfiguration));
			}

			foreach (var agent in Agents)
			{
				foreach (var rm in agent.ReadModels)
				{
					var assembly = rm.Type.Assembly;
					if (!assembliesToMap.ContainsKey(assembly))
					{
						assembliesToMap.Add(assembly, assembly);
					}
				}
			}

			foreach (var agent in assembliesToMap.Keys)
			{
				mcfg.AutoMappings.Add(AutoMap.Assembly(agent, autoMapperConfiguration).IgnoreBase<DefaultReadModel>());
			}

			return mcfg;
		}
	}
}