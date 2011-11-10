using System;
using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	public class VotingSchemeController : Controller
	{
		public ActionResult List(Guid forumId)
		{
			return View();
		}
	}
}