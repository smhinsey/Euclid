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
			var model = _forumQueries.GetForumVotingScheme(forumId);

			return View(model);
		}

		//public JsonResult SetVotingScheme(Guid forumId, VotingScheme selectedScheme)
		//{
		//    var publicationId = _publisher.PublishMessage(new UpdateForumVotingScheme
		//                                                    {
		//                                                        ForumIdentifier = forumId,
		//                                                        NoVoting = selectedScheme == VotingScheme.NoVoting,
		//                                                        UpDownVoting = selectedScheme == VotingScheme.UpDownVoting
		//                                                    });

		//    return Json(new {publicationId}, JsonRequestBehavior.AllowGet);
		//}
	}
}