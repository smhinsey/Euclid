using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.PortableAreas;
using MvcContrib.UI.InputBuilder;

namespace Euclid.Sdk.NugetTests
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            PortableAreaRegistration.RegisterEmbeddedViewEngine();

            RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterRoutes(RouteTable.Routes);

            WebRole.GetInstance().Init();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            //routes.MapRoute(
            //                "Default", "{controller}/{action}/{id}",
            //                new { controller = "Post", action = "List", id = UrlParameter.Optional });
        }
    }
}