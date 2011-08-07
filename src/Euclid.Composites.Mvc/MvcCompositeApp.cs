using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Agent;
using Euclid.Agent.Extensions;
using Euclid.Common.Logging;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.Binders;
using Euclid.Composites.Mvc.ComponentRegistration;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Mvc
{
	public class MvcCompositeApp : ILoggingSource
	{
		public readonly IList<IAgentMetadata> Agents = new List<IAgentMetadata>();
		private static readonly IWindsorContainer Container = new WindsorContainer();

		//private readonly ICommandToIInputModelConversionRegistry _commandToIInputModelConversionRegistry = new CommandToIInputModelConversionRegistry();
		private readonly IInputModelTransfomerRegistry _inputModelTransformers =
			new InputModelToCommandTransformerRegistry();

		public MvcCompositeApp()
		{
			ApplicationState = CompositeApplicationState.Uninitailized;
		}

		public CompositeApplicationState ApplicationState { get; private set; }

		public void Configure(HttpApplication mvcApplication, MvcCompositeAppSettings compositeAppSettings)
		{
			Container.Install(new ContainerInstaller());

			RegisterConfiguredTypes(compositeAppSettings);

			Container.Register(
			                   Component
			                   	.For<IInputModelTransfomerRegistry>()
			                   	.Instance(_inputModelTransformers)
			                   	.LifeStyle.Singleton);

			Container.Install(new ControllerContainerInstaller());

			RegisterModelBinders();

			ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(Container));

			mvcApplication.Error += LogUnhandledException;

			mvcApplication.BeginRequest += BeginPageRequest;

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

			_inputModelTransformers.Add(commandMetadata.Name, converter);
		}

		private void BeginPageRequest(object sender, EventArgs eventArgs)
		{
			if (ApplicationState != CompositeApplicationState.Configured)
			{
				throw new InvalidCompositeApplicationStateException(ApplicationState, CompositeApplicationState.Configured);
			}
		}

		private void LogUnhandledException(object sender, EventArgs eventArgs)
		{
			var e = HttpContext.Current.Server.GetLastError();
			this.WriteFatalMessage(e.Message, e);
		}

		private static void RegisterConfiguredTypes(MvcCompositeAppSettings compositeAppSettings)
		{
			Container.Register(
			                   Component.For<IPublisher>()
			                   	.ImplementedBy(compositeAppSettings.Publisher.Value)
			                   	.LifeStyle.Singleton
				);

			Container.Register(
			                   Component.For<IMessageChannel>()
			                   	.ImplementedBy(compositeAppSettings.MessageChannel.Value)
			                   	.LifeStyle.Singleton
				);

			Container.Register(
			                   Component.For<IRecordMapper<CommandPublicationRecord>>()
			                   	.ImplementedBy(compositeAppSettings.CommandPublicationRecordMapper.Value)
			                   	.LifeStyle.Singleton
				);

			Container.Register(
			                   Component.For<IBlobStorage>()
			                   	.ImplementedBy(compositeAppSettings.BlobStorage.Value)
			                   	.LifeStyle.Singleton
				);

			Container.Register(
			                   Component.For<IMessageSerializer>()
			                   	.ImplementedBy(compositeAppSettings.MessageSerializer.Value)
			                   	.LifeStyle.Singleton
				);

			Container.Register(
			                   Component.For<IPublicationRegistry<IPublicationRecord>>()
			                   	.ImplementedBy(compositeAppSettings.PublicationRegistry.Value)
			                   	.LifeStyle.Singleton
				);

			Container.Register(
			                   Component
			                   	.For<ICommandDispatcher>()
			                   	.ImplementedBy(compositeAppSettings.CommandDispatcher.Value)
			                   	.LifeStyle.Singleton);
		}

		private static void RegisterModelBinders()
		{
			Container.Install(new ModelBinderIntstaller());

			ModelBinders.Binders.DefaultBinder = new EuclidDefaultBinder(Container.ResolveAll<IEuclidModelBinder>());
		}
	}
}