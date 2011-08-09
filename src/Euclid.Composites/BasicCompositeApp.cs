using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Euclid.Agent;
using Euclid.Agent.Extensions;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Conversion;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata;

namespace Euclid.Composites
{
	public class BasicCompositeApp : ICompositeApp
	{
		protected BasicCompositeApp()
		{
			ApplicationState = CompositeApplicationState.Uninitailized;
			Agents = new List<IAgentMetadata>();
			InputModelTransformers = new InputModelToCommandTransformerRegistry();
			Container = new WindsorContainer();
		}

		public IList<IAgentMetadata> Agents { get; private set; }

		public CompositeApplicationState ApplicationState { get; set; }
		protected IWindsorContainer Container { get; set; }
		protected IInputModelTransfomerRegistry InputModelTransformers { get; private set; }

		public virtual void Configure(CompositeAppSettings compositeAppSettings)
		{
			Container.Kernel.Resolver.AddSubResolver(new ArrayResolver(Container.Kernel));

			Container.Kernel.Resolver.AddSubResolver(new ListResolver(Container.Kernel));

			RegisterConfiguredTypes(compositeAppSettings);

			Container.Register(Component
			                   	.For<IInputModelTransfomerRegistry>()
			                   	.Instance(InputModelTransformers)
			                   	.LifeStyle.Singleton);

			ApplicationState = CompositeApplicationState.Configured;
		}

		public void InstallAgent(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			if (!assembly.ContainsAgent())
			{
				throw new AssemblyNotAgentException(assembly);
			}

			Agents.Add(assembly.GetAgentMetadata());
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

		protected void RegisterConfiguredTypes(CompositeAppSettings compositeAppSettings)
		{
			Container.Register(Component.For<IPublisher>()
			                   	.ImplementedBy(compositeAppSettings.Publisher.Value)
			                   	.LifeStyle.Singleton);

			Container.Register(Component.For<IMessageChannel>()
			                   	.ImplementedBy(compositeAppSettings.MessageChannel.Value)
			                   	.LifeStyle.Singleton);

			Container.Register(Component.For<IRecordMapper<CommandPublicationRecord>>()
			                   	.ImplementedBy(compositeAppSettings.CommandPublicationRecordMapper.Value)
			                   	.LifeStyle.Singleton);

			Container.Register(Component.For<IBlobStorage>()
			                   	.ImplementedBy(compositeAppSettings.BlobStorage.Value)
			                   	.LifeStyle.Singleton);

			Container.Register(Component.For<IMessageSerializer>()
			                   	.ImplementedBy(compositeAppSettings.MessageSerializer.Value)
			                   	.LifeStyle.Singleton);

			Container.Register(Component.For<IPublicationRegistry<IPublicationRecord>>()
			                   	.ImplementedBy(compositeAppSettings.PublicationRegistry.Value)
			                   	.LifeStyle.Singleton);

			Container.Register(Component
			                   	.For<ICommandDispatcher>()
			                   	.ImplementedBy(compositeAppSettings.CommandDispatcher.Value)
			                   	.LifeStyle.Singleton);
		}
	}
}