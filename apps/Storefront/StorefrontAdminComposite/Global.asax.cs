using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StorefrontAdminComposite
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

			routes.MapRoute(
				"Default",
				"{controller}/{action}",
				new { controller = "Shell", action = "Index" });
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