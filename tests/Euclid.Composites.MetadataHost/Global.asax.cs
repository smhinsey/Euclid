using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Composites.MetadataHost.EuclidConfiguration;
using Euclid.Composites.Mvc;
using Euclid.Framework.Cqrs;
using Euclid.Framework.TestingFakes.Cqrs;
using Euclid.Framework.TestingFakes.InputModels;

namespace Euclid.Composites.MetadataHost
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterRoutes(RouteTable.Routes);

            var composite = new EuclidComposite();

            composite.Configure<DefaultPublisher, InMemoryMessageChannel, CommandRegistry, CompositeCommandPublicationRecordMapper, InMemoryBlobStorage, JsonMessageSerializer, CommandDispatcher>(this);

            composite.InstallAgent(typeof(FakeCommand4).Assembly);

            composite.RegisterInputModel<InputModelFakeCommand4, FakeCommand4>();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute
            //    (
            //     "Default", // Route name
            //     "{controller}/{action}/{id}", // URL with parameters
            //     new { controller = "Post", action = "List", id = UrlParameter.Optional } // Parameter defaults
            //    );
        }
    }
}