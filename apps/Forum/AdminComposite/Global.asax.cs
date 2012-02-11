using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AdminComposite
{
	public class MvcApplication : HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("favicon.ico");
			routes.IgnoreRoute("composite/{*route}");
			routes.IgnoreRoute("composite/{*pathInfo}");

			routes.MapRoute(
				"Default", "{controller}/{action}/{forumId}", 
				new { controller = "Authentication", action = "Signin", forumId = UrlParameter.Optional });
		}

		protected void Application_Start()
		{
			RegisterGlobalFilters(GlobalFilters.Filters);

			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

			WebRole.GetInstance().Init();
		}
	}
}