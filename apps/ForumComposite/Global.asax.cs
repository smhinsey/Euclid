using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ForumComposite.ActionFilters;

namespace ForumComposite
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
				"Default", "org/{orgSlug}/forum/{forumSlug}/{controller}/{action}/{id}", new { controller = "Post", action = "List", id = UrlParameter.Optional, orgSlug = Guid.NewGuid(), forumSlug = Guid.NewGuid() });
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			GlobalFilters.Filters.Add(new PopulateForumWideViewBag());

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			WebRole.GetInstance().Init();
		}
	}
}