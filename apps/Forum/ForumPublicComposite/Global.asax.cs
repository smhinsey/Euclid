using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Euclid.Common.Logging;

namespace ForumPublicComposite
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication, ILoggingSource
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("favicon.ico");
			routes.IgnoreRoute("composite/{*pathInfo}");

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
				new { controller = "PostListing", action = "Popular", page = UrlParameter.Optional }
				);

			routes.MapRoute(
				"AllPosts",
				"org/{org}/forum/{forum}/posts/all",
				new { controller = "PostListing", action = "All", page = UrlParameter.Optional }
				);

			routes.MapRoute(
				"ControversialPosts",
				"org/{org}/forum/{forum}/posts/controversial",
				new { controller = "PostListing", action = "Controversial", page = UrlParameter.Optional }
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
				"org/{org}/forum/{forum}/posts/{postSlug}/{postIdentifier}",
				new { controller = "Post", action = "Detail" }
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

			routes.MapRoute(
				"AuthenticateSignIn",
				"org/{org}/forum/{forum}/authenticate",
				new { controller = "Authentication", action = "Authenticate" }
				);

			routes.MapRoute(
				"AuthenticateSignOut",
				"org/{org}/forum/{forum}/authenticate/signout",
				new { controller = "Authentication", action = "SignOut" }
				);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			WebRole.GetInstance().Init();
		}


		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				return;
			}

			var route = RouteTable.Routes.GetRouteData(new HttpContextWrapper(Context));

			if (route == null || route.Route.GetType().Name == "IgnoreRouteInternal")
			{
				return;
			}

			var requestedOrg = route.GetRequiredString("org").Trim().ToLowerInvariant();
			var requestedForum = route.GetRequiredString("forum").Trim().ToLowerInvariant();

			var identity = (FormsIdentity)Context.User.Identity;

			var rawData = identity.Ticket.UserData;

			if (rawData.Contains("^"))
			{
				var orgForumCombo = rawData.Split(new[] { "^" }, StringSplitOptions.None);

				if (orgForumCombo.Length < 2)
				{
					this.WriteErrorMessage("User {0}'s forms authentication ticket did not contain org/forum data.", identity.Name);

					redirectOnFailure(requestedOrg, requestedForum);
				}

				var userOrg = orgForumCombo[0];
				var userForum = orgForumCombo[1];

				if (userOrg != requestedOrg || userForum != requestedForum)
				{
					this.WriteErrorMessage("User {0}'s forms authentication ticket was for another org/forum.", identity.Name);

					redirectOnFailure(requestedOrg, requestedForum);
				}
			}
			else
			{
				this.WriteErrorMessage("User {0}'s forms authentication ticket did not contain org/forum data.", identity.Name);

				redirectOnFailure(requestedOrg, requestedForum);
			}
		}

		private void redirectOnFailure(string org, string forum)
		{
			var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

			if (authCookie != null)
			{
				authCookie.Value = null;
				authCookie.Path = string.Format("/org/{0}/forum/{1}", org, forum);
				authCookie.Expires = DateTime.Now.AddYears(-1);

				Response.Cookies.Add(authCookie);
			}

			Response.Redirect(string.Format("/org/{0}/forum/{1}", org, forum));
		}
	}
}