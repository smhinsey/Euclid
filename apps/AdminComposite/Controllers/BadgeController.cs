using System;
using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class BadgeController : Controller
	{
		public ActionResult List(Guid? forumId)
		 {
		 	return View();
		 }

		public PartialViewResult NewBadge(Guid? forumId)
		{
			return PartialView("_NewBadge");
		}
	}
}