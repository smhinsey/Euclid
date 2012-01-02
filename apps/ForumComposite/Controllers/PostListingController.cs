using System.Web.Mvc;
using ForumAgent.Queries;
using ForumComposite.ViewModels.PostListing;

namespace ForumComposite.Controllers
{
	public class PostListingController : ForumController
	{
		private readonly PostQueries _postQueries;

		public PostListingController(PostQueries postQueries)
		{
			_postQueries = postQueries;
		}

		public ActionResult All()
		{
			var model = new AllPostsViewModel { Listing = _postQueries.FindAllPosts(ForumIdentifier, 16, 0) };

			return View(model);
		}

		public ActionResult Controversial()
		{
			var model = new ControversialPostsViewModel { Listing = _postQueries.FindControversialPosts(ForumIdentifier, 16, 0) };

			return View(model);
		}

		public ActionResult Popular()
		{
			var model = new PopularPostsViewModel { Listing = _postQueries.FindPopularPosts(ForumIdentifier, 10, 0) };

			return View(model);
		}
	}
}