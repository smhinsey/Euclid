using System.Web.Mvc;

namespace Euclid.Composites.MvcApplication.Controllers
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