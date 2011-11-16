using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Euclid.Common.Configuration;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.NHibernate;
using Euclid.Common.Storage.Record;
using Euclid.Composites.Formatters;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;
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
		private readonly IList<IAgentMetadata> _agents;

		private IInputModelMapCollection _inputModelMap;

		public BasicCompositeApp()
		{
			_agents = new List<IAgentMetadata>();

			State = CompositeApplicationState.Uninitailized;
			Container = new WindsorContainer();
		}

		public BasicCompositeApp(IWindsorContainer container)
			: this()
		{
			Container = container;
		}

		public IEnumerable<IAgentMetadata> Agents
		{
			get { return _agents; }
		}

		public string Description { get; set; }

		public IEnumerable<ITypeMetadata> InputModels
		{
			get
			{ return _inputModelMap.InputModels; }
		}

		public IEnumerable<IPartMetadata> Commands
		{
			get { return _inputModelMap.Commands; }
		}

		public string Name { get; set; }

		public CompositeAppSettings Settings { get; set; }

		public CompositeApplicationState State { get; set; }

		protected IWindsorContainer Container { get; set; }

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

			var agent = assembly.GetAgentMetadata();

			_agents.Add(agent);

			// SELF the Where call below changes the meaning of the rest of the registration so it had to be removed
			Container.Register(
				AllTypes.FromAssembly(agent.AgentAssembly)
					// .Where(Component.IsInNamespace(agent.Queries.Namespace))
					.BasedOn(typeof (IQuery)).WithService.Self().Configure(component => component.LifeStyle.Transient));
		}

		public virtual void Configure(CompositeAppSettings compositeAppSettings)
		{
			Container.Kernel.Resolver.AddSubResolver(new ArrayResolver(Container.Kernel));

			Container.Kernel.Resolver.AddSubResolver(new ListResolver(Container.Kernel));

			RegisterConfiguredTypes(compositeAppSettings);

			Container.Register(Component.For<IWindsorContainer>().Instance(Container));

			Container.Register(Component.For<ICompositeApp>().Instance(this));

			Settings = compositeAppSettings;

			Settings.Validate();

			_inputModelMap = Settings.InputModelMaps.Value;

			State = CompositeApplicationState.Configured;
		}

		public void CreateSchema(IPersistenceConfigurer databaseConfiguration, bool destructive)
		{
			if (destructive)
			{
				Fluently.Configure().Database(databaseConfiguration).Mappings(map => mapAllAssemblies(map)).ExposeConfiguration(
					cfg => new SchemaExport(cfg).Create(false, true)).BuildSessionFactory();
			}
			else
			{
				Fluently.Configure().Database(databaseConfiguration).Mappings(map => mapAllAssemblies(map)).ExposeConfiguration(
					cfg => new SchemaUpdate(cfg).Execute(false, true)).BuildSessionFactory();
			}
		}

		public IPartMetadata GetCommandMetadataForInputModel(Type inputModelType)
		{
			if (!typeof (IInputModel).IsAssignableFrom(inputModelType))
			{
				throw new InvalidTypeSettingException(inputModelType.FullName, typeof (IInputModel), inputModelType.GetType());
			}

			var map = Mapper.GetAllTypeMaps().Where(m => m.SourceType == inputModelType).FirstOrDefault();

			if (map == null)
			{
				throw new InputModelNotRegisteredException(inputModelType);
			}

			var commandType = map.DestinationType;

			return commandType.GetMetadata() as PartMetadata;
		}

		public Type GetInputModelTypeForCommandName(string commandName)
		{
			return _inputModelMap.GetInputModelTypeForCommandName(commandName);
		}

		public IEnumerable<string> GetConfigurationErrors()
		{
			var allErrors = new List<string>();

			if (string.IsNullOrEmpty(Name))
			{
				allErrors.Add("The composite is not named");
			}

			if (string.IsNullOrEmpty(Description))
			{
				allErrors.Add("The composite has no description");
			}

			var settingErrors = Settings.GetInvalidSettingReasons();

			allErrors.AddRange(settingErrors);

			return allErrors;
		}

		public IMetadataFormatter GetFormatter()
		{
			return new CompositeMetadataFormatter(this);
		}

		public bool IsValid()
		{
			return (GetConfigurationErrors().Count() == 0 && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description));
		}

		public void RegisterInputModelMap<TInputModelSource, TCommandDestination>()
			where TInputModelSource : IInputModel
			where TCommandDestination : ICommand
		{
			_inputModelMap.RegisterInputModel<TInputModelSource, TCommandDestination>();
		}

		public void RegisterInputModelMap<TInputModelSource, TCommandDestination>(
			Func<TInputModelSource, TCommandDestination> customMap)
			where TInputModelSource : IInputModel
			where TCommandDestination : ICommand
		{
			_inputModelMap.RegisterInputModel(customMap);
		}

		public void RegisterNh(IPersistenceConfigurer databaseConfiguration, bool isWeb)
		{
			var lifestyleType = isWeb ? LifestyleType.PerWebRequest : LifestyleType.Transient;

			Container.Register(
				Component.For<ISessionFactory>().UsingFactoryMethod<ISessionFactory>(
					() =>
					Fluently.Configure().Database(databaseConfiguration).Mappings(map => mapAllAssemblies(map)).BuildSessionFactory()).
					LifeStyle.Singleton);

			// jt: open session should be read-only
			Container.Register(
				Component.For<ISession>().UsingFactoryMethod<ISession>(() => Container.Resolve<ISessionFactory>().OpenSession()).
					LifeStyle.Is(lifestyleType));
		}

		protected void RegisterConfiguredTypes(CompositeAppSettings compositeAppSettings)
		{
			Container.Register(
				Component.For<IPublisher>().ImplementedBy(compositeAppSettings.Publisher.Value).LifeStyle.Transient);

			Container.Register(
				Component.For<IMessageChannel>().ImplementedBy(compositeAppSettings.OutputChannel.Value).LifeStyle.Transient);

			Container.Register(
				Component.For<IRecordMapper<CommandPublicationRecord>>().ImplementedBy(
					compositeAppSettings.CommandPublicationRecordMapper.Value).LifeStyle.Transient);

			Container.Register(
				Component.For<IBlobStorage>().ImplementedBy(compositeAppSettings.BlobStorage.Value).LifeStyle.Transient);

			Container.Register(
				Component.For<IMessageSerializer>().ImplementedBy(compositeAppSettings.MessageSerializer.Value).LifeStyle.Transient);

			Container.Register(
				Component.For<IPublicationRegistry<IPublicationRecord, IPublicationRecord>>().Forward<ICommandRegistry>().
					ImplementedBy(compositeAppSettings.PublicationRegistry.Value).LifeStyle.Transient);
		}

		private MappingConfiguration mapAllAssemblies(MappingConfiguration mcfg)
		{
			var autoMapperConfiguration = new AutoMapperConfiguration();

			var assembliesToMap = new Dictionary<Assembly, Assembly>();

			if (Settings.CommandPublicationRecordMapper.Value == typeof (NhRecordMapper<CommandPublicationRecord>))
			{
				mcfg.AutoMappings.Add(
					AutoMap.AssemblyOf<CommandPublicationRecord>(autoMapperConfiguration).Conventions.Add
						<DefaultStringLengthConvention>());
			}

			foreach (var agent in Agents)
			{
				foreach (var rm in agent.ReadModels.Collection)
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
				// SELF we need to shift to shipping these with the agents and using classmaps for them so we have better control
				mcfg.AutoMappings.Add(
					AutoMap
						.Assembly(agent, autoMapperConfiguration)
						.IgnoreBase<DefaultReadModel>()
						.IgnoreBase<UnpersistedReadModel>()
						.Conventions
						.Add<DefaultStringLengthConvention>());

				mcfg.FluentMappings.AddFromAssembly(agent);
			}

			return mcfg;
		}
	}
}