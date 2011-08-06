using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Agent;
using Euclid.Agent.Commands;
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
    public class EuclidMvcComposite : ILoggingSource
    {
        private static readonly IWindsorContainer Container = new WindsorContainer();

        public readonly IList<IAgentMetadata> Agents = new List<IAgentMetadata>();

        //private readonly ICommandToIInputModelConversionRegistry _commandToIInputModelConversionRegistry = new CommandToIInputModelConversionRegistry();
        private readonly IInputModelTransfomerRegistry _inputModelTransformers = new InputModelToCommandTransformerRegistry();
        
        public EuclidMvcComposite()
        {
            ApplicationState = CompositeApplicationState.Uninitailized;
        }

        public CompositeApplicationState ApplicationState { get; private set; }

        public void Configure
            <TPublisher, TMessageChannel, TPublicationRegistry, TCommandPublicationRecordMapper, TBlobStorage,
             TMessageSerializer, TCommandDispatcher>(HttpApplication mvcApplication)
            where TPublisher : IPublisher
            where TMessageChannel : IMessageChannel
            where TPublicationRegistry : IPublicationRegistry<IPublicationRecord>
            where TCommandPublicationRecordMapper : IRecordMapper<CommandPublicationRecord>
            where TBlobStorage : IBlobStorage
            where TMessageSerializer : IMessageSerializer
            where TCommandDispatcher : ICommandDispatcher
        {
            Container.Install(new ContainerInstaller());

            Container.Register(
                Component.For<IPublisher>()
                    .ImplementedBy<TPublisher>()
                    .LifeStyle.Singleton
                );

            Container.Register(
                Component.For<IMessageChannel>()
                    .ImplementedBy<TMessageChannel>()
                    .LifeStyle.Singleton
                );

            Container.Register(
                Component.For<IRecordMapper<CommandPublicationRecord>>()
                    .ImplementedBy(typeof (TCommandPublicationRecordMapper))
                    .LifeStyle.Singleton
                );

            Container.Register(
                Component.For<IBlobStorage>()
                    .ImplementedBy<TBlobStorage>()
                    .LifeStyle.Singleton
                );

            Container.Register(
                Component.For<IMessageSerializer>()
                    .ImplementedBy<TMessageSerializer>()
                    .LifeStyle.Singleton
                );

            Container.Register(
                Component.For<IPublicationRegistry<IPublicationRecord>>()
                    .ImplementedBy<TPublicationRegistry>()
                    .LifeStyle.Singleton
                );

            Container.Register(
                Component
                    .For<ICommandDispatcher>()
                    .ImplementedBy<TCommandDispatcher>()
                    .LifeStyle.Singleton);

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

            ApplicationState = CompositeApplicationState.ReadyToStart;
        }

        private void BeginPageRequest(object sender, EventArgs eventArgs)
        {
            if (ApplicationState != CompositeApplicationState.Running)
            {
                throw new InvalidCompositeApplicationStateException(ApplicationState, CompositeApplicationState.Running);
            }
        }

        private void LogUnhandledException(object sender, EventArgs eventArgs)
        {
            var e = HttpContext.Current.Server.GetLastError();
            this.WriteFatalMessage(e.Message, e);
        }

        public void InstallAgent(Assembly assembly)
        {
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

        public void Start()
        {
            ApplicationState = CompositeApplicationState.Running;
        }

        public void Stop()
        {
            ApplicationState = CompositeApplicationState.Stopping;

            ApplicationState = CompositeApplicationState.Stopped;
        }

        private static void RegisterModelBinders()
        {
            Container.Install(new ModelBinderIntstaller());

            ModelBinders.Binders.DefaultBinder = new EuclidDefaultBinder(Container.ResolveAll<IEuclidModelBinder>());
        }
    }
}
