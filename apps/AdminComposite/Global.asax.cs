using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AdminComposite.ActionFilters;

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

			routes.MapRoute(
				"Default", "{controller}/{action}/{forumId}", 
				new { controller = "Authentication", action = "Signin", forumId = UrlParameter.Optional });
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			GlobalFilters.Filters.Add(new PopulateAdminWideViewBag());
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			WebRole.GetInstance().Init();
		}
	}
}