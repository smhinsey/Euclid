using System;
using System.Web.Mvc;
using AdminComposite.Models;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	public class ForumController : Controller
	{
		private readonly ForumQueries _forumQueries;

		public ForumController(ForumQueries forumQueries)
		{
			_forumQueries = forumQueries;
		}

		public ActionResult Create()
		{
			return View(new CreateForumInputModel());
		}

		public ActionResult List()
		{
			var forums = _forumQueries.List(0, 100);

			return View(forums);
		}

		public ActionResult Details(Guid forumId)
		{
			var forum = _forumQueries.FindById(forumId);

			return View(forum);
		}
	}
}