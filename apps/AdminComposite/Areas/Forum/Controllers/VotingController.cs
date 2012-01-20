using System;
using System.Web.Mvc;
using ForumAgent.Queries;

namespace AdminComposite.Areas.Forum.Controllers
{
	public class VotingController : Controller
	{
		private readonly ForumQueries _forumQueries;
		public VotingController(ForumQueries forumQueries)
		{
			_forumQueries = forumQueries;
		}

		public ActionResult List(Guid forumId)
		{
			var model = _forumQueries.GetForumVotingScheme(forumId);

			return View(model);
		}
	}
}