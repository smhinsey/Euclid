using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AgentPanel.Areas.Metadata.Models;
using BoC.Web.Mvc.PrecompiledViews;

namespace ForumComposite
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			ApplicationPartRegistry.Register(typeof (AgentModel).Assembly);

			AreaRegistration.RegisterAllAreas();

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

			routes.MapRoute(
			                "Default", "{controller}/{action}/{id}", 
			                new {controller = "Post", action = "List", id = UrlParameter.Optional});
		}
	}
}
