using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class ModerationController : Controller
	{
		public ActionResult Comments()
		{
			ViewBag.ModerationType = "Comment";
			ViewBag.Title = "Moderate Comments";
			return View("Moderation");
		}

		public ActionResult Posts()
		{
			ViewBag.ModerationType = "Post";
			ViewBag.Title = "Moderate Posts";
			return View("Moderation");
		}
	}
}