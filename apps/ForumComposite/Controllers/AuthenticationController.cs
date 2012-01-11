using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ForumAgent.Queries;

namespace ForumComposite.Controllers
{
	public class AuthenticationController : ForumController
	{
		private readonly UserQueries _userQueries;

		public AuthenticationController(UserQueries userQueries)
		{
			_userQueries = userQueries;
		}

		public ActionResult Authenticate(string org, string forum, string username, string password)
		{
			if (_userQueries.Authenticate(ForumInfo.ForumIdentifier, username, password))
			{
				var user = _userQueries.FindByUsername(ForumInfo.ForumIdentifier, username);

				// TODO: re-enable this when everything works
				//Publisher.PublishMessage(
				//  new UpdateForumUserLastLogin
				//  {
				//    Created = DateTime.Now,
				//    CreatedBy = user.Identifier,
				//    Identifier = Guid.NewGuid(),
				//    LoginTime = DateTime.Now,
				//    UserIdentifier = user.Identifier
				//  });

				var userData = string.Format("{0}^{1}^{2}", org, forum, user.Identifier);

				var authCookie = FormsAuthentication.GetAuthCookie(user.Username, true);
				var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

				var ticket = new FormsAuthenticationTicket(authTicket.Version, authTicket.Name, authTicket.IssueDate, authTicket.Expiration, authTicket.IsPersistent, userData);
				var encryptedTicket = FormsAuthentication.Encrypt(ticket);

				authCookie.Value = encryptedTicket;
				authCookie.Path = string.Format("/org/{0}/forum/{1}", org, forum);
				authCookie.Expires = DateTime.Now.AddDays(10);

				Response.Cookies.Add(authCookie);

				return new RedirectToRouteResult("Home", null);
			}

			// TODO: redirect to a login error screen
			return new RedirectToRouteResult("Home", null);
		}

		public ActionResult SignOut(string org, string forum)
		{
			var authCookie = FormsAuthentication.GetAuthCookie(ForumInfo.AuthenticatedUserName, true);

			authCookie.Value = null;
			authCookie.Path = string.Format("/org/{0}/forum/{1}", org, forum);
			authCookie.Expires = DateTime.Now.AddYears(-1);

			Response.Cookies.Add(authCookie);

			return new RedirectToRouteResult("Home", null);
		}
	}
}