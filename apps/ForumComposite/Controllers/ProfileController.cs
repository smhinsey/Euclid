using System.Web.Mvc;
using ForumAgent.Queries;
using ForumComposite.ViewModels.Profile;

namespace ForumComposite.Controllers
{
	public class ProfileController : ForumController
	{
		private readonly PostQueries _postQueries;

		private readonly UserQueries _userQueries;

		public ProfileController(UserQueries userQueries, PostQueries postQueries)
		{
			_userQueries = userQueries;
			_postQueries = postQueries;
		}

		public ActionResult All()
		{
			return View();
		}

		public ActionResult Badges(string profileSlug)
		{
			var model = new ProfileBadgesViewModel
				{ User = _userQueries.FindByUsername(ForumIdentifier, profileSlug), IsCurrentUser = profileSlug == CurrentUserName };

			return View(model);
		}

		public ActionResult Favorites(string profileSlug)
		{
			var model = new ProfileFavoritesViewModel
				{ User = _userQueries.FindByUsername(ForumIdentifier, profileSlug), IsCurrentUser = profileSlug == CurrentUserName };

			return View(model);
		}

		public ActionResult Friends(string profileSlug)
		{
			var model = new ProfileFriendsViewModel
				{ User = _userQueries.FindByUsername(ForumIdentifier, profileSlug), IsCurrentUser = profileSlug == CurrentUserName };

			return View(model);
		}

		public ActionResult Overview(string profileSlug)
		{
			var model = new ProfileOverviewViewModel
				{ User = _userQueries.FindByUsername(ForumIdentifier, profileSlug), IsCurrentUser = profileSlug == CurrentUserName };

			return View(model);
		}

		public ActionResult RecentActivity(string profileSlug)
		{
			var model = new ProfileRecentActivityViewModel
				{ User = _userQueries.FindByUsername(ForumIdentifier, profileSlug), IsCurrentUser = profileSlug == CurrentUserName };

			return View(model);
		}
	}
}