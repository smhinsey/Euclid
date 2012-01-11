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

				var issueDate = DateTime.Now;

				var expirationDate = DateTime.Now.AddYears(1);

				var userData = string.Format("{0}^{1}^{2}", org, forum, user.Identifier);

				var ticket = new FormsAuthenticationTicket(1, user.Username, issueDate, expirationDate, true, userData);

				var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
					{ Path = string.Format("org/{0}/forum/{1}", org, forum), Expires = expirationDate };

				Response.AppendCookie(cookie);

				return new RedirectToRouteResult("Home", null);
			}

			// TODO: redirect to a login error screen
			return new RedirectToRouteResult("Home", null);
		}

		public ActionResult SignOut(string org, string forum)
		{
			FormsAuthentication.SignOut();

			Response.Cookies.Remove(FormsAuthentication.FormsCookieName);

			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
				{ Path = string.Format("org/{0}/forum/{1}", org, forum), Expires = DateTime.Now.AddYears(-10) };

			Response.Cookies.Add(cookie);

			return new RedirectToRouteResult("Home", null);
		}
	}
}