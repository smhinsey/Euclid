using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Agent;
using Euclid.Composites.Mvc.Binders;
using Euclid.Composites.Mvc.ComponentRegistration;

namespace Euclid.Composites.Mvc
{
    public class EuclidComposite
    {
        private static readonly IWindsorContainer Container = new WindsorContainer();

        private static readonly BiDirectionalMapperCollection Maps = new BiDirectionalMapperCollection();

        public EuclidComposite()
        {
        }

        public void Configure()
        {
            Container.Install(new ContainerInstaller());

            Container.Install(new ControllerContainerInstaller());

            Container.Register(
                Component
                    .For<BiDirectionalMapperCollection>()
                    .Instance(Maps)
                    .LifeStyle.Singleton);

            RegisterModelBinders();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(Container));

            RegisterControllers();
        }

        public static void AddMap<T, TPrime>(IBiDirectionalMapper<T, TPrime> map)
        {
            Maps.Add(map);
        }

        // called by agent package as it's installed via nuget
        public static void InstallAgent(Assembly agent)
        {
            //retrieve metadata defined in AgentInfo.cs

            var attributes = agent.GetCustomAttributes(typeof (AgentNameAttribute), false);

            if (attributes.Length > 0)
            {
                var agentName = attributes[0];
            }
        }

        private static void RegisterModelBinders()
        {
            Container.Install(new ModelBinderIntstaller());

            ModelBinders.Binders.DefaultBinder = new EuclidDefaultBinder(Container.ResolveAll<IEuclidModelBinder>());           
        }

        private static void RegisterControllers()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoute(
                "Command",
                "{controller}/{action}/{scheme}+{systemName}/{command}",
                new { controller = "Command", action = "Inspect", command = UrlParameter.Optional }
            );

            // JT: add route for query controller when it exists
        }
    }
}
