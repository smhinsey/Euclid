using System.Web.Mvc;
using ForumAgent.Queries;
using ForumComposite.ViewModels.Profile;

namespace ForumComposite.Controllers
{
	public class ProfileController : ForumController
	{
		private readonly UserQueries _userQueries;

		public ProfileController(UserQueries userQueries)
		{
			_userQueries = userQueries;
		}

		public ActionResult All()
		{
			return View();
		}

		public ActionResult Badges(string profileSlug)
		{
			var model = new ProfileBadgesViewModel
				{
					User = _userQueries.FindByUsername(CurrentForum.ForumIdentifier, profileSlug),
					IsCurrentUser = profileSlug == CurrentForum.CurrentUserName
				};

			return View(model);
		}

		public ActionResult Favorites(string profileSlug)
		{
			var model = new ProfileFavoritesViewModel
				{
					User = _userQueries.FindByUsername(CurrentForum.ForumIdentifier, profileSlug),
					IsCurrentUser = profileSlug == CurrentForum.CurrentUserName,
				};

			model.Favorites = _userQueries.FindUserFavorites(CurrentForum.ForumIdentifier, model.User.Identifier);

			return View(model);
		}

		public ActionResult Friends(string profileSlug)
		{
			var model = new ProfileFriendsViewModel
				{
					User = _userQueries.FindByUsername(CurrentForum.ForumIdentifier, profileSlug),
					IsCurrentUser = profileSlug == CurrentForum.CurrentUserName
				};

			model.Friends = _userQueries.FindUserFriends(CurrentForum.ForumIdentifier, model.User.Identifier);

			return View(model);
		}

		public ActionResult Overview(string profileSlug)
		{
			var model = new ProfileOverviewViewModel
				{
					User = _userQueries.FindByUsername(CurrentForum.ForumIdentifier, profileSlug),
					IsCurrentUser = profileSlug == CurrentForum.CurrentUserName
				};

			return View(model);
		}

		public ActionResult RecentActivity(string profileSlug)
		{
			var model = new ProfileRecentActivityViewModel
				{
					User = _userQueries.FindByUsername(CurrentForum.ForumIdentifier, profileSlug),
					IsCurrentUser = profileSlug == CurrentForum.CurrentUserName
				};

			model.Activity = _userQueries.FindUserActivity(CurrentForum.ForumIdentifier, model.User.Identifier);

			return View(model);
		}
	}
}