using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Composite.MvcApplication.EuclidConfiguration;
using Euclid.Composite.MvcApplication.EuclidConfiguration.TypeConverters;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using Euclid.Framework.Cqrs;
using Euclid.Framework.TestingFakes.Cqrs;
using Euclid.Framework.TestingFakes.InputModels;

namespace Euclid.Composite.MvcApplication
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class AgentViewerComposite : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterRoutes(RouteTable.Routes);

            var composite = new EuclidMvcComposite();

            composite.Configure(this, new EuclidMvcConfiguration());

            composite.InstallAgent(typeof (FakeCommand4).Assembly);

            composite.RegisterInputModel(new InputToFakeCommand4Converter());
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Command",
                "{controller}/{action}/{agentSystemName}/{commandName}",
                new { controller = "Command", action = "Inspect", commandName = UrlParameter.Optional }
                );

            routes.MapRoute(
                "InspectCommand",
                "{agentSystemName}/{commandName}.{format}",
                new { controller = "Command", action = "Inspect", format = UrlParameter.Optional }
                );
        }
    }
}