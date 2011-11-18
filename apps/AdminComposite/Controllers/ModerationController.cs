using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class ModerationController : Controller
	{
		private readonly ModeratedPostQueries _postQueries;
		private readonly IPublisher _publisher;

		public ModerationController(ModeratedPostQueries postQueries, IPublisher publisher)
		{
			_postQueries = postQueries;
			_publisher = publisher;
		}

		public ActionResult Comments(Guid? forumId)
		{
			ViewBag.ModerationType = "Comment";
			ViewBag.Title = "Moderate Forum Comments";
			return View("Moderation");
		}

		public ActionResult Posts(Guid forumId, int offset = 0, int pageSize = 5)
		{
			ViewBag.ModerationType = "Post";
			ViewBag.Title = "Moderate Forum Posts";

			return View("Moderation", _postQueries.ListUnapprovedPosts(forumId, offset, pageSize));
		}

		public JsonResult ApprovePost(Guid postId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);
			var publicationId = _publisher.PublishMessage(new ApprovePost
			                                              	{
			                                              		ApprovedBy = userId,
			                                              		CreatedBy = userId,
			                                              		PostIdentifier = postId
			                                              	});

			return Json(publicationId, JsonRequestBehavior.AllowGet);
		}

		public JsonResult RejectPost(Guid postId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);
			var publicationId = _publisher.PublishMessage(new RejectPost
			                                              	{
			                                              		CreatedBy = userId,
			                                              		PostIdentifier = userId
			                                              	});

			return Json(publicationId, JsonRequestBehavior.AllowGet);
		}
	}
}