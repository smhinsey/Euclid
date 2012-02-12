using System;
using System.Web.Mvc;
using AdminComposite.Areas.Forum.InputModels;
using AdminComposite.Controllers;
using ForumAgent.Queries;

namespace AdminComposite.Areas.Forum.Controllers
{
	[Authorize]
	public class AvatarController : AdminController
	{
		private readonly AvatarQueries _avatarQueries;

		public AvatarController(AvatarQueries avatarQueries)
		{
			_avatarQueries = avatarQueries;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			var model = _avatarQueries.FindAvatarsForForum(forumId, offset, pageSize);

			ViewBag.Pagination = new PaginationModel
				{
					ActionName = "List",
					ControllerName = "Avatar",
					Identifier = forumId,
					Offset = offset,
					PageSize = pageSize,
					TotalItems = model.TotalAvatars,
				};

			return View(model);
		}

		public PartialViewResult NewAvatar(Guid forumId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			return PartialView("_NewAvatar", new CreateForumAvatarInputModel { ForumIdentifier = forumId, CreatedBy = userId, });
		}

		public PartialViewResult UpdateAvatar(Guid avatarId)
		{
			var avatar = _avatarQueries.FindById(avatarId);

			return PartialView(
				"_UpdateAvatar",
				new UpdateForumAvatarInputModel
					{ AvatarIdentifier = avatarId, Description = avatar.Description, Name = avatar.Name, ImageUrl = avatar.Url });
		}
	}
}