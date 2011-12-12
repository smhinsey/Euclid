using System;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
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