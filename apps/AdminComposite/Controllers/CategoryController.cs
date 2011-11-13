using System;
using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class CategoryController : Controller
	{
		public ActionResult List(Guid forumId)
		{
			return View();
		}

		public PartialViewResult NewCategory()
		{
			return PartialView("_NewCategory");
		}
	}
}