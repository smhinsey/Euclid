using System.Web.Mvc;

namespace ForumSkeletonMvc.Controllers
{
	public class PostListingController : Controller
	{
		public ActionResult All()
		{
			return View();
		}

		public ActionResult Controversial()
		{
			return View();
		}

		public ActionResult Popular()
		{
			return View();
		}
	}
}