using System.Web.Mvc;
using ForumComposite.ViewModels.PostListing;

namespace ForumComposite.Controllers
{
	public class PostListingController : Controller
	{
		public ActionResult All()
		{
			var model = new AllPostsViewModel();

			return View(model);
		}

		public ActionResult Controversial()
		{
			var model = new ControversialPostsViewModel();

			return View(model);
		}

		public ActionResult Popular()
		{
			var model = new PopularPostsViewModel();

			return View(model);
		}
	}
}