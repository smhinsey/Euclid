using System;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class AvatarController : Controller
	{
		private readonly AvatarQueries _avatarQueries;
		private readonly IPublisher _publisher;
		public AvatarController(AvatarQueries avatarQueries, IPublisher publisher)
		{
			_avatarQueries = avatarQueries;
			_publisher = publisher;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			var model = _avatarQueries.FindAvatarsForForum(forumId, offset, pageSize);

			ViewBag.Pagination = new PaginationModel
			                     	{
			                     		ActionName = "List",
			                     		ControllerName = "Avatar",
			                     		ForumIdentifier = forumId,
			                     		Offset = offset,
			                     		PageSize = pageSize,
			                     		TotalItems = model.TotalAvatars,
			                     		WriteTFoot = false,
			                     		WriteTable = false,
			                     		WriteTr = false
			                     	};

			return View(model.Avatars);
		}

		public JsonResult ActivateAvatar(Guid avatarId, bool active)
		{
			var publicationId = _publisher.PublishMessage(new ActivateAvatar
			                                              	{
			                                              		Active = active,
			                                              		AvatarIdentifier = avatarId
			                                              	});

			return Json(new {publicationId}, JsonRequestBehavior.AllowGet);
		}

		public PartialViewResult UpdateAvatar(Guid avatarId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			throw new NotImplementedException();
			// return PartialView("_UpdateAvatar", new UpdateForumAvatarInputModel)
		}

		public PartialViewResult NewAvatar(Guid forumId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			return PartialView("_NewAvatar", new CreateForumAvatarInputModel
			                                 	{
			                                 		ForumIdentifier = forumId,
													CreatedBy = userId,
			                                 	});
		}
	}
}