using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Agent;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;
using Euclid.Composites.Extensions;
using Euclid.Composites.Mvc.Binders;
using Euclid.Composites.Mvc.ComponentRegistration;
using Euclid.Composites.Mvc.MappingPipeline;
using Euclid.Composites.Mvc.Maps;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc
{
    public class EuclidComposite
    {
        private static readonly IWindsorContainer Container = new WindsorContainer();

        public MapperRegistry Mappers { get;private set; }


        public EuclidComposite()
        {
            Mappers = new MapperRegistry();
        }

        public void Configure<TPublisher, TMessageChannel, TPublicationRegistry, TCommandPublicationRecordMapper, TBlobStorage, TMessageSerializer>() 
            where TPublisher : IPublisher
            where TMessageChannel : IMessageChannel
            where TPublicationRegistry : IPublicationRegistry<IPublicationRecord>
            where TCommandPublicationRecordMapper : IRecordMapper<CommandPublicationRecord>
            where TBlobStorage : IBlobStorage 
            where TMessageSerializer : IMessageSerializer
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
                    .ImplementedBy(typeof(TCommandPublicationRecordMapper))
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

            Container.Install(new ControllerContainerInstaller());

            Container.Register(
                Component
                    .For<MapperRegistry>()
                    .Instance(Mappers)
                    .LifeStyle.Singleton);

            RegisterModelBinders();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(Container));

            RegisterRoutes();

            Mappers.Add(new DefaultComponentMetadataToInputModelMap());
        }

        // called by agent package as it's installed via nuget
        public void InstallAgent(Assembly assembly)
        {
            if (!assembly.ContainsAgent())
            {
                throw new AssemblyNotAgentException(assembly);
            }

            // register agent queries

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
                new { controller = "Command", action = "Inspect", command = UrlParameter.Optional }
            );

            RouteTable.Routes.MapRoute(
                "InspectCommand",
                "{agentSystemName}/{commandName}.{format}",
                new { controller = "Command", action = "Inspect", format = UrlParameter.Optional }
                );

            // JT: add route for query controller when it exists
        }
    }
}