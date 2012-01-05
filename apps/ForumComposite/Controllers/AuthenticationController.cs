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
				//    CreatedBy = Guid.Empty,
				//    Identifier = Guid.NewGuid(),
				//    LoginTime = DateTime.Now,
				//    UserIdentifier = user.Identifier
				//  });

				var ticket = new FormsAuthenticationTicket(
					1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(30), true, string.Format("{0}^{1}", org, forum));

				var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
					{ Path = string.Format("org/{0}/forum/{1}", org, forum) };

				Response.AppendCookie(cookie);

				return new RedirectToRouteResult("Home", null);
			}

			// redirect to a login error screen
			return new RedirectToRouteResult("Home", null);
		}

		public ActionResult Signout()
		{
			FormsAuthentication.SignOut();

			return new RedirectToRouteResult("Home", null);
		}
	}
}