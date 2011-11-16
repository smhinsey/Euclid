using System;
using System.Web.Mvc;
using AdminComposite.Models;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	public class VotingSchemeController : Controller
	{
		private readonly ForumQueries _forumQueries;

		public VotingSchemeController(ForumQueries forumQueries)
		{
			_forumQueries = forumQueries;
		}

		public ActionResult List(Guid forumId)
		{
			var forum = _forumQueries.FindById(forumId);
			return View(new SetVotingSchemeInputModel
			            	{
			            		ForumIdentifier = forum.Identifier,
								SelectedScheme = forum.NoVoting ? SetVotingSchemeInputModel.AvailableScheme.NoVoting : SetVotingSchemeInputModel.AvailableScheme.UpDownVoting
			            	});
		}
	}
}