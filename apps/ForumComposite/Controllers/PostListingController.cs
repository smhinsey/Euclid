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

		public ActionResult All(int? page)
		{
			var offset = page.GetValueOrDefault(1);

			offset--;

			offset = offset * 16;

			var model = new AllPostsViewModel
				{
					Listing = _postQueries.FindAllPosts(ForumIdentifier, 16, offset),
					CurrentPage = page.GetValueOrDefault(1)
				};

			return View(model);
		}

		public ActionResult Controversial(int? page)
		{
			var offset = page.GetValueOrDefault(1);

			offset--;

			offset = offset * 16;

			var model = new ControversialPostsViewModel
				{
					Listing = _postQueries.FindControversialPosts(ForumIdentifier, 16, offset),
					CurrentPage = page.GetValueOrDefault(1)
				};

			return View(model);
		}

		public ActionResult Popular(int? page)
		{
			var offset = page.GetValueOrDefault(1);

			offset--;

			offset = offset * 10;

			var model = new PopularPostsViewModel
				{
					Listing = _postQueries.FindPopularPosts(ForumIdentifier, 10, offset),
					CurrentPage = page.GetValueOrDefault(1)
				};

			return View(model);
		}
	}
}