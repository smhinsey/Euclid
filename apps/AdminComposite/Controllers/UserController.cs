using System;
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

		public ActionResult Created()
		{
			return View();
		}

		public ActionResult Details(Guid? forumId)
		{
			return View();
		}

		public ActionResult Invite(Guid? forumId)
		{
			return View();
		}

		public ActionResult List(Guid? forumId)
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

		public ActionResult ForgotPassword()
		{
			return View();
		}
	}
}