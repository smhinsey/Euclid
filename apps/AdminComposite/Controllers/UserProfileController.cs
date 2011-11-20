using System;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class UserProfileController : Controller
	{
		private readonly UserQueries _forumUserQueries;
		private readonly IPublisher _publisher;

		public UserProfileController(UserQueries forumUserQueries, IPublisher publisher)
		{
			_forumUserQueries = forumUserQueries;
			_publisher = publisher; 
		}

		public ActionResult Details(Guid? forumId)
		{
			return View("_Details");
		}

		public ActionResult Invite(Guid forumId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			return View("_Invite", new RegisterForumUserInputModel
			                       	{
			                       		ForumIdentifier = forumId,
			                       		Password = "password", //TODO: better password generation required
										CreatedBy = userId
			                       	});
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			return View(_forumUserQueries.FindByForum(Guid.Parse(ViewBag.CurrentForumId), offset, pageSize));
		}

		[HttpPost]
		public JsonResult PerformBlockOperation(Guid userIdentifier)
		{
			var user = _forumUserQueries.FindById(userIdentifier);

			var message = user.IsBlocked
			              	? (IMessage) new UnblockUser {UserIdentifier = userIdentifier}
			              	: (IMessage) new BlockUser {UserIdentifier = userIdentifier};

			return Json(new { publicationId = _publisher.PublishMessage(message) });
		}

		public JsonResult BlockStatus(Guid userIdentifier)
		{
			var user = _forumUserQueries.FindById(userIdentifier);

			return Json(new {isBlocked = user.IsBlocked}, JsonRequestBehavior.AllowGet);
		}
	}
}