using System.Web.Mvc;
using System.Web.Security;
using ForumAgent.Queries;
using ForumComposite.Models;

namespace ForumComposite.Controllers
{
	public class ProfileController : Controller
	{
		private readonly UserQueries _userQueries;

		public ProfileController(UserQueries userQueries)
		{
			_userQueries = userQueries;
		}

		public ActionResult SignIn()
		{
			return View();
		}

		public ActionResult SignOut()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("List", "Post");
		}

		[HttpPost]
		public ActionResult Authenticate(string username, string password)
		{
			if (_userQueries.Authenticate(username, password))
			{
				FormsAuthentication.SetAuthCookie(username, false);

				return RedirectToAction("List", "Post");
			}
			
			return RedirectToAction("SignIn");
		}

		public ActionResult Register()
		{
			return View(new RegisterUserInputModel());
		}
	}
}