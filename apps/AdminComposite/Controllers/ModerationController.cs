using System;
using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class ModerationController : Controller
	{
		public ActionResult Comments(Guid? forumId)
		{
			ViewBag.ModerationType = "Comment";
			ViewBag.Title = "Moderate Forum Comments";
			return View("Moderation");
		}

		public ActionResult Posts(Guid? forumId)
		{
			ViewBag.ModerationType = "Post";
			ViewBag.Title = "Moderate Forum Posts";
			return View("Moderation");
		}
	}
}