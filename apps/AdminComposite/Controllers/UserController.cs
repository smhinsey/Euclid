using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AdminComposite.Models;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	public class UserController : Controller
	{
		private readonly OrganizationUserQueries _userQueries;

		public UserController(OrganizationUserQueries userQueries)
		{
			_userQueries = userQueries;
		}

		public ActionResult Create()
		{
			ViewBag.Title = "New User";
			return View(new CreateOrganizationAndUserInputModel());
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

		[HttpPost]
		public ActionResult Signin(string username, string password)
		{
			if (_userQueries.AutenticateOrganizationUser(username, password))
			{
				var user = _userQueries.FindByUsername(username);

				// SELF need to do something better here
				Response.Cookies.Add(new HttpCookie("OrganizationUserId", user.Identifier.ToString()));

				FormsAuthentication.SetAuthCookie(username, false);

				return RedirectToAction("Index", "Dashboard");
			}

			ViewBag.Error = "Wrong Username and Password combination.";
			return View("SignIn");
		}

		public ActionResult Signout()
		{
			Response.Cookies.Remove("OrganizationUserId");
			FormsAuthentication.SignOut();
			return View();
		}

		public ActionResult ForgotPassword()
		{
			return View();
		}
	}
}