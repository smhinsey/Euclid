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
		private readonly IPublisher _publisher;

		public VotingController(ForumQueries forumQueries, IPublisher publisher)
		{
			_forumQueries = forumQueries;
			_publisher = publisher;
		}

		public ActionResult List(Guid forumId)
		{
			var forum = _forumQueries.FindById(forumId);
			ViewBag.FormName = forum.Name;

			return View(new SetVotingSchemeInputModel
			            	{
			            		ForumIdentifier = forum.Identifier,
								SelectedScheme = forum.NoVoting ? VotingScheme.NoVoting : VotingScheme.UpDownVoting
			            	});
		}

		public JsonResult SetVotingScheme(Guid forumId, VotingScheme selectedScheme)
		{
			var publicationId = _publisher.PublishMessage(new UpdateForumVotingScheme
			                                              	{
			                                              		ForumIdentifier = forumId,
			                                              		NoVoting = selectedScheme == VotingScheme.NoVoting,
			                                              		UpDownVoting = selectedScheme == VotingScheme.UpDownVoting
			                                              	});

			return Json(new {publicationId}, JsonRequestBehavior.AllowGet);
		}
	}
}