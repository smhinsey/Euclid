using System;
using System.Web.Mvc;
using ForumAgent.Queries;
using ForumComposite.ViewModels.Post;

namespace ForumComposite.Controllers
{
	public class PostController : ForumController
	{
		private readonly PostQueries _postQueries;

		public PostController(PostQueries postQueries)
		{
			_postQueries = postQueries;
		}

		public ActionResult Detail(string postSlug, Guid postIdentifier)
		{
			var model = new PostDetailViewModel { Post = _postQueries.FindByIdentifier(ForumIdentifier, postIdentifier) };


			return View(model);
		}
	}
}