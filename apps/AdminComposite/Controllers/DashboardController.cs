using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class DashboardController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}