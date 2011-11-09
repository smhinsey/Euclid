using System;
using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class ContentController : Controller
	{
		public ActionResult ForForum(Guid forumId)
		{
			return View();
		}

		public PartialViewResult NewContent(Guid forumId)
		{
			return PartialView();
		}
	}
}