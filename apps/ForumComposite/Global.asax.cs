using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
				"Home",
				"org/{org}/forum/{forum}",
				new { controller = "PostListing", action = "Popular" }
				);

			routes.MapRoute(
				"Register",
				"org/{org}/forum/{forum}/register",
				new { controller = "Account", action = "Register" }
				);

			routes.MapRoute(
				"PopularPosts",
				"org/{org}/forum/{forum}/posts/popular",
				new { controller = "PostListing", action = "Popular" }
				);

			routes.MapRoute(
				"AllPosts",
				"org/{org}/forum/{forum}/posts/all",
				new { controller = "PostListing", action = "All" }
				);

			routes.MapRoute(
				"ControversialPosts",
				"org/{org}/forum/{forum}/posts/controversial",
				new { controller = "PostListing", action = "Controversial" }
				);

			routes.MapRoute(
				"Categories",
				"org/{org}/forum/{forum}/categories",
				new { controller = "Category", action = "All" }
				);

			routes.MapRoute(
				"Category",
				"org/{org}/forum/{forum}/categories/{categorySlug}",
				new { controller = "Category", action = "Detail" }
				);


			routes.MapRoute(
				"Tags",
				"org/{org}/forum/{forum}/tags",
				new { controller = "Tag", action = "All" }
				);

			routes.MapRoute(
				"Tag",
				"org/{org}/forum/{forum}/tags/{tagSlug}",
				new { controller = "Tag", action = "Detail" }
				);

			routes.MapRoute(
				"AllProfiles",
				"org/{org}/forum/{forum}/profiles",
				new { controller = "Profile", action = "All" }
				);

			routes.MapRoute(
				"ProfileOverview",
				"org/{org}/forum/{forum}/profiles/{profileSlug}",
				new { controller = "Profile", action = "Overview" }
				);

			routes.MapRoute(
				"ProfileBadges",
				"org/{org}/forum/{forum}/profiles/{profileSlug}/badges",
				new { controller = "Profile", action = "Badges" }
				);

			routes.MapRoute(
				"ProfileFavorites",
				"org/{org}/forum/{forum}/profiles/{profileSlug}/favorites",
				new { controller = "Profile", action = "Favorites" }
				);

			routes.MapRoute(
				"ProfileFriends",
				"org/{org}/forum/{forum}/profiles/{profileSlug}/friends",
				new { controller = "Profile", action = "Friends" }
				);

			routes.MapRoute(
				"ProfileRecentActivity",
				"org/{org}/forum/{forum}/profiles/{profileSlug}/activity",
				new { controller = "Profile", action = "RecentActivity" }
				);

			routes.MapRoute(
				"Post",
				"org/{org}/forum/{forum}/categories/{categorySlug}/posts/{postSlug}",
				new
				{
					controller = "Post",
					action = "Detail",
					categorySlug = UrlParameter.Optional,
					postSlug = UrlParameter.Optional
				}
				);

			routes.MapRoute(
				"CreatePost",
				"org/{org}/forum/{forum}/createpost",
				new { controller = "Post", action = "Create" }
				);

			routes.MapRoute(
				"InfoPage",
				"org/{org}/forum/{forum}/pages/info",
				new { controller = "Pages", action = "Info" }
				);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			WebRole.GetInstance().Init();
		}
	}
}