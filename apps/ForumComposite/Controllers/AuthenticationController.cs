using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ForumAgent.Queries;
using ForumComposite.Models;

namespace ForumComposite.Controllers
{
	public class AuthenticationController : Controller
	{
		private readonly UserQueries _userQueries;

		public AuthenticationController(UserQueries userQueries)
		{
			_userQueries = userQueries;
		}

		[HttpPost]
		public ActionResult Authenticate(string username, string password)
		{
			if (_userQueries.Authenticate(ViewBag.ForumIdentifier, username, password))
			{
				var user = _userQueries.FindByUsername(ViewBag.ForumIdentifier, username);

				// SELF need to do something better here
				Response.Cookies.Add(new HttpCookie("ForumUserId", user.Identifier.ToString()));

				FormsAuthentication.SetAuthCookie(username, false, "");

				return RedirectToAction("List", "Post", new { forumId = user.ForumIdentifier });
			}

			return RedirectToAction("SignIn");
		}

		public ActionResult Register()
		{
			return View(new RegisterForumUserInputModel { ForumIdentifier = ViewBag.ForumIdentifier });
		}

		public ActionResult SignIn()
		{
			return View();
		}

		public ActionResult SignOut()
		{
			FormsAuthentication.SignOut();

			Response.Cookies.Remove("ForumUserId");

			return RedirectToAction("List", "Post");
		}
	}
}