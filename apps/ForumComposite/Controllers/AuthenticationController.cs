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

		public ActionResult Authenticate(string username, string password)
		{
			if (_userQueries.Authenticate(ForumInfo.ForumIdentifier, username, password))
			{
				var user = _userQueries.FindByUsername(ForumInfo.ForumIdentifier, username);

				//Publisher.PublishMessage(
				//  new UpdateOrganizationUserLastLogin
				//  {
				//    Created = DateTime.Now,
				//    CreatedBy = Guid.Empty,
				//    Identifier = Guid.NewGuid(),
				//    LoginTime = DateTime.Now,
				//    UserIdentifier = user.Identifier
				//  });

				// SELF need to do something better here
				Response.Cookies.Add(new HttpCookie(string.Format("{0}UserId", ForumInfo.ForumName), user.Identifier.ToString()));

				FormsAuthentication.SetAuthCookie(username, false);

				return new RedirectToRouteResult("Home", null);
			}
			
			// redirect to a login error screen
			return new RedirectToRouteResult("Home", null);
		}

		public ActionResult Signout()
		{
			Response.Cookies.Remove(string.Format("{0}UserId", ForumInfo.ForumName));

			FormsAuthentication.SignOut();

			return new RedirectToRouteResult("Home", null);
		}
	}
}