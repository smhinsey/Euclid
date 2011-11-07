using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class BadgeController : Controller
	{
		 public ActionResult List()
		 {
		 	return View();
		 }

		public PartialViewResult NewBadge()
		{
			return PartialView("_NewBadge");
		}
	}
}