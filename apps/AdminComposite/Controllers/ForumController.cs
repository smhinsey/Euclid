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
			return View(new CreateForumInputModel {UrlHostName = "socialrally.com"});
		}

		public ActionResult AuthenticationProviders(Guid forumId)
		{
			return View();
		}

		public ActionResult Details(Guid forumId)
		{
			var forum = _forumQueries.FindById(forumId);

			return View(forum);
		}
	}
}