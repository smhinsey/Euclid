using System;
using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class ModerationController : Controller
	{
		public ActionResult Comments(Guid? forumId)
		{
			ViewBag.ModerationType = "Comment";
			ViewBag.Title = "Moderate Comments";
			return View("Moderation");
		}

		public ActionResult Posts(Guid? forumId)
		{
			ViewBag.ModerationType = "Post";
			ViewBag.Title = "Moderate Posts";
			return View("Moderation");
		}
	}
}