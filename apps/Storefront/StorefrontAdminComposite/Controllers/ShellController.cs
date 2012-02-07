using System.Web.Mvc;

namespace StorefrontAdminComposite.Controllers
{
	public class ShellController : AdminController
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}