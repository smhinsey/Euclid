using System.Web.Mvc;

namespace ForumComposite.Controllers
{
	public class PostController : Controller
	{
		//
		// GET: /Home/

		public ActionResult List()
		{
			return View();
		}

		public ActionResult Thread()
		{
			return View();
		}
	}
}