using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ForumSkeletonMvc
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
				"Home",
				"",
				new {controller = "PostListing", action = "Popular"}
				);

			routes.MapRoute(
				"PopularPosts",
				"posts/popular",
				new {controller = "PostListing", action = "Popular"}
				);

			routes.MapRoute(
				"AllPosts",
				"posts/all",
				new { controller = "PostListing", action = "All" }
				);

			routes.MapRoute(
				"ControversialPosts",
				"posts/controversial",
				new { controller = "PostListing", action = "Controversial" }
				);

			routes.MapRoute(
				"Categories",
				"categories",
				new { controller = "RoughDraft", action = "Categories" }
				);

			routes.MapRoute(
				"Category",
				"categories/{categorySlug}",
				new { controller = "RoughDraft", action = "Category", categorySlug = UrlParameter.Optional }
				);

			routes.MapRoute(
				"OldDefault", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new {controller = "RoughDraft", action = "Home", id = UrlParameter.Optional} // Parameter defaults
				);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
		}
	}
}