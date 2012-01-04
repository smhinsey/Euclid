using System;
using System.Web.Mvc;
using ForumAgent.Queries;
using ForumComposite.ViewModels.Post;

namespace ForumComposite.Controllers
{
	public class PostController : ForumController
	{
		private readonly PostQueries _postQueries;

		private readonly UserQueries _userQueries;

		public PostController(PostQueries postQueries, UserQueries userQueries)
		{
			_postQueries = postQueries;
			_userQueries = userQueries;
		}

		public ActionResult Detail(string postSlug, Guid postIdentifier)
		{
			var model = new PostDetailViewModel
				{
					Post = _postQueries.FindByIdentifier(ForumInfo.ForumIdentifier, postIdentifier),
					IsFavoritePost = _userQueries.IsFavoritePost(ForumInfo.ForumIdentifier, ForumInfo.AuthenticatedUserIdentifier, postIdentifier)
				};

			return View(model);
		}
	}
}