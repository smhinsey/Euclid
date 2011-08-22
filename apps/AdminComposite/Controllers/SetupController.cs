using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class SetupController : Controller
	{
		// GET: /Setup/
		public ActionResult Start()
		{
			return this.View();
		}
	}
}