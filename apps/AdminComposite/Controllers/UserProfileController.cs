using System;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class UserProfileController : Controller
	{
		private readonly OrganizationUserQueries _organizationUserQueries;
		private readonly UserQueries _forumUserQueries;
		private readonly IPublisher _commandPublisher;

		public UserProfileController(OrganizationUserQueries organizationUserQueries, IPublisher publisher, UserQueries forumUserQueries)
		{
			_organizationUserQueries = organizationUserQueries;
			_commandPublisher = publisher;
			_forumUserQueries = forumUserQueries;
		}

		public ActionResult Details(Guid? forumId)
		{
			return View("_Details");
		}

		public ActionResult Invite(Guid forumId)
		{
			return View("_Invite", new RegisterForumUserInputModel
			                       	{
			                       		ForumIdentifier = forumId,
			                       		Password = "password" //TODO: better password generation required
			                       	});
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			return View(_forumUserQueries.List(offset, pageSize));
		}
	}
}