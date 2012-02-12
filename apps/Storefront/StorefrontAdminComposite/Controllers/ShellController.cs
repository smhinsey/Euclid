using System.Web.Mvc;

namespace StorefrontAdminComposite.Controllers
{
	public class ShellController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}