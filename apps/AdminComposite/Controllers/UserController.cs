using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	public class UserController : Controller
	{
		private readonly OrganizationUserQueries _userQueries;
		private readonly IPublisher _commandPublisher;

		public UserController(OrganizationUserQueries userQueries, IPublisher publisher)
		{
			_userQueries = userQueries;
			_commandPublisher = publisher;
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

		[Authorize]
		public ActionResult Details(Guid? forumId)
		{
			return View();
		}

		[Authorize]
		public ActionResult Invite(Guid? forumId)
		{
			return View();
		}

		[Authorize]
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
				_commandPublisher.PublishMessage(new UpdateLastLogin
				                                 	{
				                                 		Created = DateTime.Now,
				                                 		CreatedBy = Guid.Empty,
				                                 		Identifier = Guid.NewGuid(),
				                                 		LoginTime = DateTime.Now,
				                                 		UserIdentifier = user.Identifier
				                                 	});

				// SELF need to do something better here
				Response.Cookies.Add(new HttpCookie("OrganizationUserId", user.Identifier.ToString()));

				FormsAuthentication.SetAuthCookie(username, false);
				return RedirectToAction("Index", "Dashboard");
			}

			ViewBag.Error = "Wrong Username and Password combination.";
			return View("SignIn");
		}

		public ActionResult DoSignout()
		{
			Response.Cookies.Remove("OrganizationUserId");
			FormsAuthentication.SignOut();

			return RedirectToAction("Signout");
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