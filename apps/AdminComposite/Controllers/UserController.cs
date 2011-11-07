using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class UserController : Controller
	{
		public ActionResult Create()
		{
			ViewBag.Title = "New User";
			return View();
		}

		public ActionResult Details()
		{
			return View();
		}

		public ActionResult Invite()
		{
			return View();
		}

		public ActionResult List()
		{
			return View();
		}

		public ActionResult Signin()
		{
			return View();
		}

		public ActionResult Signout()
		{
			return View();
		}
	}
}