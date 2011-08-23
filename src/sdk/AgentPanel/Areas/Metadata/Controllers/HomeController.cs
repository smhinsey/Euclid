using System.Web.Mvc;

namespace Euclid.Composite.MvcApplication.Controllers
{
	public class HomeController : Controller
	{
		//
		// GET: /Home/

		public ActionResult Index()
		{
			return View();
		}
	}
}