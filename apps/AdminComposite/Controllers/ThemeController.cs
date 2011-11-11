using System;
using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class ThemeController : Controller
	{
		public ActionResult List(Guid forumId)
		{
			return View();
		}
	}
}