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
				new {controller = "PostListing", action = "All"}
				);

			routes.MapRoute(
				"ControversialPosts",
				"posts/controversial",
				new {controller = "PostListing", action = "Controversial"}
				);

			routes.MapRoute(
				"Categories",
				"categories",
				new {controller = "Category", action = "All"}
				);

			routes.MapRoute(
				"Category",
				"categories/{categorySlug}",
				new {controller = "Category", action = "Detail", categorySlug = UrlParameter.Optional}
				);

			routes.MapRoute(
				"Tags",
				"tags",
				new {controller = "Tag", action = "All"}
				);

			routes.MapRoute(
				"Tag",
				"tags/{tagSlug}",
				new {controller = "Tag", action = "Detail", tagSlug = UrlParameter.Optional}
				);

			routes.MapRoute(
				"AllProfiles",
				"profiles",
				new {controller = "Profile", action = "All"}
				);

			routes.MapRoute(
				"ProfileOverview",
				"profiles/{profileSlug}",
				new {controller = "Profile", action = "Overview", profileSlug = UrlParameter.Optional}
				);

			routes.MapRoute(
				"ProfileBadges",
				"profiles/{profileSlug}/badges",
				new {controller = "Profile", action = "Badges", profileSlug = UrlParameter.Optional}
				);

			routes.MapRoute(
				"ProfileFavorites",
				"profiles/{profileSlug}/favorites",
				new {controller = "Profile", action = "Favorites", profileSlug = UrlParameter.Optional}
				);

			routes.MapRoute(
				"ProfileFriends",
				"profiles/{profileSlug}/friends",
				new {controller = "Profile", action = "Friends", profileSlug = UrlParameter.Optional}
				);

			routes.MapRoute(
				"ProfileRecentActivity",
				"profiles/{profileSlug}/activity",
				new {controller = "Profile", action = "RecentActivity", profileSlug = UrlParameter.Optional}
				);

			routes.MapRoute(
				"Post",
				"categories/{categorySlug}/posts/{postSlug}",
				new
					{
						controller = "Post",
						action = "Detail",
						categorySlug = UrlParameter.Optional,
						postSlug = UrlParameter.Optional
					}
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