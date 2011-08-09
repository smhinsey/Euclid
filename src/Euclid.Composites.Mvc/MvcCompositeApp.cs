using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
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

namespace Euclid.Composites.Mvc
{
	public class MvcCompositeApp : DefaultCompositeApp, ILoggingSource
	{
		private static readonly IWindsorContainer Container = new WindsorContainer();

		private readonly IInputModelTransfomerRegistry _inputModelTransformers =
			new InputModelToCommandTransformerRegistry();

		public MvcCompositeApp()
		{
			ApplicationState = CompositeApplicationState.Uninitailized;
		}

		public void Configure(HttpApplication mvcApplication, MvcCompositeAppSettings compositeAppSettings)
		{
			Container.Install(new ContainerInstaller());

			registerConfiguredTypes(compositeAppSettings);

			Container.Register(Component
			                   	.For<IInputModelTransfomerRegistry>()
			                   	.Instance(_inputModelTransformers)
			                   	.LifeStyle.Singleton);

			Container.Install(new ControllerContainerInstaller());

			registerModelBinders();

			ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(Container));

			mvcApplication.Error += logUnhandledException;

			mvcApplication.BeginRequest += beginPageRequest;

			ApplicationState = CompositeApplicationState.Configured;
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

		private void registerConfiguredTypes(MvcCompositeAppSettings compositeAppSettings)
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

		private void registerModelBinders()
		{
			Container.Install(new ModelBinderInstaller());

			ModelBinders.Binders.DefaultBinder = new EuclidDefaultBinder(Container.ResolveAll<IEuclidModelBinder>());
		}

		private void beginPageRequest(object sender, EventArgs eventArgs)
		{
			if (ApplicationState != CompositeApplicationState.Configured)
			{
				throw new InvalidCompositeApplicationStateException(ApplicationState, CompositeApplicationState.Configured);
			}
		}

		private void logUnhandledException(object sender, EventArgs eventArgs)
		{
			var e = HttpContext.Current.Server.GetLastError();
			this.WriteFatalMessage(e.Message, e);
		}
	}
}