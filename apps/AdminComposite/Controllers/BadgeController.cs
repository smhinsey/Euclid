using System;
using System.Web.Mvc;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class BadgeController : Controller
	{
		private readonly ForumQueries _forumQueries;

		public BadgeController(ForumQueries forumQueries)
		{
			_forumQueries = forumQueries;
		}

		public ActionResult List(Guid forumId)
		{
			ViewBag.ForumName = _forumQueries.FindById(forumId).Name;

			return View();
		}

		public PartialViewResult NewBadge(Guid forumId)
		{
			return PartialView("_NewBadge");
		}
	}
}