using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Agent;
using Euclid.Composites.Extensions;
using Euclid.Composites.Mvc.Binders;
using Euclid.Composites.Mvc.ComponentRegistration;
using Euclid.Composites.Mvc.MappingPipeline;

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

        public void Configure()
        {
            Container.Install(new ContainerInstaller());

            Container.Install(new ControllerContainerInstaller());

            Container.Register(
                Component
                    .For<MapperRegistry>()
                    .Instance(Mappers)
                    .LifeStyle.Singleton);

            RegisterModelBinders();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(Container));

            RegisterControllers();
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
