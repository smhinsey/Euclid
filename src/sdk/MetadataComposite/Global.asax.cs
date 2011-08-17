using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Composites;
using Euclid.Composites.Mvc;

namespace MetadataComposite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterRoutes(RouteTable.Routes);

            var container = new WindsorContainer();

            var composite = new MvcCompositeApp(container);

            var euclidCompositeConfiguration = new CompositeAppSettings();

            /*
             * jt: this is how a composite developer would override the default settings for the mvc composite
             * euclidCompositeConfiguration.BlobStorage.ApplyOverride(typeof(SomeBlobStorageImplementation));
             */

            composite.Configure(euclidCompositeConfiguration);

            /*
            jt: this is going to be injected into composites adding this pacage
            
            composite.AddAgent(typeof(FakeCommand).Assembly);
            
            composite.RegisterInputModel(new InputToFakeCommand4Converter());
            */

            container.Register(Component.For<ICompositeApp>().Instance(composite));

            Error += composite.LogUnhandledException;

            BeginRequest += composite.BeginPageRequest;
        }
    }
}