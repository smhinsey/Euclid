using System.Web.Mvc;
using ForumComposite.Models;

namespace ForumComposite.Controllers
{
	public class AccountController : Controller
	{
		public ActionResult Register()
		{
			return View(new RegisterForumUserInputModel());
		}

		public ActionResult SignIn()
		{
			return View();
		}

		public ActionResult SignOut()
		{
			return View();
		}
	}
}