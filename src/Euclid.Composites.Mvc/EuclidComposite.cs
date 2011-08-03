using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Agent;
using Euclid.Common.Logging;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;
using Euclid.Composites.Mvc.Binders;
using Euclid.Composites.Mvc.ComponentRegistration;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata;
using Euclid.Framework.Metadata.Extensions;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc
{
    public enum CompositeApplicationState
    {
        Uninitailized = 0,
        ReadyToStart,
        Running,
        Stopping,
        Stopped
    }

    public class EuclidComposite : ILoggingSource
    {
        private static readonly IWindsorContainer Container = new WindsorContainer();

        public readonly IList<IAgentMetadata> Agents = new List<IAgentMetadata>();

        private readonly ICommandToInputModelConversionRegistry _commandToInputModelConversionRegistry = new CommandToInputModelConversionRegistry();

        private ICommandDispatcher _dispatcher;

        public EuclidComposite()
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
                    .For<ICommandToInputModelConversionRegistry>()
                    .Instance(_commandToInputModelConversionRegistry)
                    .LifeStyle.Singleton);

            Container.Register(
                Component
                    .For<ICommandDispatcher>()
                    .ImplementedBy<TCommandDispatcher>()
                    .LifeStyle.Singleton);

            Container.Install(new ControllerContainerInstaller());

            RegisterModelBinders();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(Container));

            RegisterRoutes();

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

        public void RegisterInputModel<TInputModel, TCommand>()
            where TInputModel : IInputModel
            where TCommand : ICommand
        {
            _commandToInputModelConversionRegistry.AddTypeConversion<TCommand, TInputModel>();
        }

        public void Start()
        {
            // jt: i think this is where the fabric and dispatchers should get started
            /*
             * 
            _dispatcher = Container.Resolve<ICommandDispatcher>();
            var settings = new MessageDispatcherSettings();

            settings.InputChannel.WithDefault(new InMemoryMessageChannel());
            settings.InvalidChannel.WithDefault(new InMemoryMessageChannel());
            settings.MessageProcessorTypes.WithDefault(new List<Type> {typeof (FakeMessageProcessor)});
            settings.DurationOfDispatchingSlice.WithDefault(new TimeSpan(0, 0, 0, 0, 200));
            settings.NumberOfMessagesToDispatchPerSlice.WithDefault(30);

            _dispatcher.Configure(settings);

            _dispatcher.Enable();

            _transport.Open();
             *  
             */
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

        private static void RegisterRoutes()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoute(
                "Command",
                "{controller}/{action}/{agentSystemName}/{commandName}",
                new {controller = "Command", action = "Inspect", commandName = UrlParameter.Optional}
                );

            RouteTable.Routes.MapRoute(
                "InspectCommand",
                "{agentSystemName}/{commandName}.{format}",
                new {controller = "Command", action = "Inspect", format = UrlParameter.Optional}
                );
        }
    }

    internal class InvalidCompositeApplicationStateException : Exception
    {
        private readonly CompositeApplicationState _applicationState;
        private readonly CompositeApplicationState _expectedState;

        public InvalidCompositeApplicationStateException(CompositeApplicationState applicationState,
                                                         CompositeApplicationState expectedState)
        {
            _applicationState = applicationState;
            _expectedState = expectedState;
        }
    }


    public interface ITypeConversionRegistry<out TDestinationInterface>
    {
        void AddTypeConversion(Type sourceMetadata, Type destinationType);
        void AddTypeConversion<TSource, TDestination>() where TSource : IAgentPart;

        TDestinationInterface Convert(string nameOfSource);
        TDestinationInterface Convert(ITypeMetadata sourceMetadata);
    }

    public interface ICommandToInputModelConversionRegistry : ITypeConversionRegistry<IInputModel>
    {
    }

    public class CommandToInputModelConversionRegistry : ICommandToInputModelConversionRegistry
    {
        private readonly IDictionary<string, Type> _commandToInputMapType = new Dictionary<string, Type>();

        public void AddTypeConversion(Type commandType, Type inputMapType)
        {
            var commandMetadata = commandType.GetMetadata();

            GuardCommandMetadata(commandMetadata);

            GuardInputModel(inputMapType);

            _commandToInputMapType.Add(commandMetadata.Name, inputMapType);
        }

        public void AddTypeConversion<TSource, TDestionation>() where TSource : IAgentPart
        {
            AddTypeConversion(typeof (TSource), typeof (TDestionation));
        }

        public IInputModel Convert(string commandName)
        {
            return Activator.CreateInstance(_commandToInputMapType[commandName]) as IInputModel;
        }

        public IInputModel Convert(ITypeMetadata commandMetadata)
        {
            return Convert(commandMetadata.Name);
        }

        private void GuardInputModel(Type inputModelType)
        {
            if (!typeof (IInputModel).IsAssignableFrom(inputModelType))
            {
                throw new UnexpectedTypeException(inputModelType);
            }
        }

        private void GuardCommandMetadata(ITypeMetadata commandMetadata)
        {
            if (!typeof (ICommand).IsAssignableFrom(commandMetadata.Type))
            {
                throw new UnexpectedTypeException(commandMetadata.Type);
            }

            if (_commandToInputMapType.ContainsKey(commandMetadata.Name))
            {
                throw new CommandAlreadyMappedException(commandMetadata.Type);
            }
        }
    }

    internal class UnexpectedTypeException : Exception
    {
        private readonly Type _inputModelType;

        public UnexpectedTypeException(Type inputModelType)
        {
            _inputModelType = inputModelType;
        }
    }

    public class CommandAlreadyMappedException : Exception
    {
        private readonly Type _commandType;

        public CommandAlreadyMappedException(Type commandType)
        {
            _commandType = commandType;
        }
    }
}