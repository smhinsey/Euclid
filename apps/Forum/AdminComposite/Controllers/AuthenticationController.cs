using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AdminComposite.Areas.Forum.InputModels;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	public class AuthenticationController : AdminController
	{
		private readonly OrganizationUserQueries _organizationUserQueries;
		private readonly IPublisher _commandPublisher;

		public AuthenticationController(OrganizationUserQueries organizationUserQueries, IPublisher commandPublisher)

		{
			_organizationUserQueries = organizationUserQueries;
			_commandPublisher = commandPublisher;
		}

		public ActionResult Create()
		{
			ViewBag.Title = "New User";
			return View(new CreateOrganizationAndRegisterUserInputModel());
		}

		public ActionResult ForgotPassword()
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
			if (_organizationUserQueries.AutenticateOrganizationUser(username, password))
			{
				var user = _organizationUserQueries.FindByUsername(username);
				_commandPublisher.PublishMessage(
					new UpdateOrganizationUserLastLogin
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

			return View();
		}

		public ActionResult Signout()
		{
			Response.Cookies.Remove("OrganizationUserId");
			FormsAuthentication.SignOut();

			return View();
		}
	}
}