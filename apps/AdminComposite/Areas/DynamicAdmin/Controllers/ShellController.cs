using System.Web.Mvc;

namespace AdminComposite.Areas.DynamicAdmin.Controllers
{
	public class ShellController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}