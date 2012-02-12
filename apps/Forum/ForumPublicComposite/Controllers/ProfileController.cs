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

		public ActionResult All(int? page)
		{
			var offset = page.GetValueOrDefault(1);

			offset--;

			offset = offset * 10;

			var model = new AllProfilesViewModel { UserListing = _userQueries.FindForumUsers(ForumInfo.ForumIdentifier, offset, 10)};

			return View(model);
		}

		public ActionResult Badges(string profileSlug)
		{
			var model = new ProfileBadgesViewModel
				{
					User = _userQueries.FindByUsername(ForumInfo.ForumIdentifier, profileSlug),
					IsCurrentUser = profileSlug == ForumInfo.AuthenticatedUserName
				};

			return View(model);
		}

		public ActionResult Favorites(string profileSlug)
		{
			var model = new ProfileFavoritesViewModel
				{
					User = _userQueries.FindByUsername(ForumInfo.ForumIdentifier, profileSlug),
					IsCurrentUser = profileSlug == ForumInfo.AuthenticatedUserName,
				};

			model.Favorites = _userQueries.FindUserFavorites(ForumInfo.ForumIdentifier, model.User.Identifier);

			return View(model);
		}

		public ActionResult Friends(string profileSlug)
		{
			var model = new ProfileFriendsViewModel
				{
					User = _userQueries.FindByUsername(ForumInfo.ForumIdentifier, profileSlug),
					IsCurrentUser = profileSlug == ForumInfo.AuthenticatedUserName
				};

			model.Friends = _userQueries.FindUserFriends(ForumInfo.ForumIdentifier, model.User.Identifier);

			return View(model);
		}

		public ActionResult Overview(string profileSlug)
		{
			var model = new ProfileOverviewViewModel
				{
					User = _userQueries.FindByUsername(ForumInfo.ForumIdentifier, profileSlug),
					IsCurrentUser = profileSlug == ForumInfo.AuthenticatedUserName
				};

			return View(model);
		}

		public ActionResult RecentActivity(string profileSlug)
		{
			var model = new ProfileRecentActivityViewModel
				{
					User = _userQueries.FindByUsername(ForumInfo.ForumIdentifier, profileSlug),
					IsCurrentUser = profileSlug == ForumInfo.AuthenticatedUserName
				};

			model.Activity = _userQueries.FindUserActivity(ForumInfo.ForumIdentifier, model.User.Identifier);

			return View(model);
		}
	}
}