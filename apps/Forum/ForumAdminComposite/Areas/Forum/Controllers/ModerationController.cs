using System;
using System.Globalization;
using System.Web.Mvc;
using AdminComposite.Areas.Forum.InputModels;
using AdminComposite.Controllers;
using AdminComposite.Extensions;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite.Areas.Forum.Controllers
{
	[Authorize]
	public class ModerationController : AdminController
	{
		private readonly ModeratedCommentQueries _commentQueries;

		private readonly ModeratedPostQueries _postQueries;

		public ModerationController(ModeratedPostQueries postQueries, ModeratedCommentQueries commentQueries)
		{
			_postQueries = postQueries;
			_commentQueries = commentQueries;
		}

		public ActionResult Items(Guid forumId, int offset = 0, int pageSize = 5, string type = "posts")
		{
			ViewBag.Title = "Moderate Forum " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(type);
			ViewBag.ItemType = type;

			ModeratedItems model = null;
			if (type.ToLower() == "posts")
			{
				model = _postQueries.ListUnapprovedPosts(forumId, offset, pageSize);
			}
			else if (type.ToLower() == "comments")
			{
				model = _commentQueries.ListUnapprovedComments(forumId, offset, pageSize);
			}
			else
			{
				throw new ArgumentOutOfRangeException("type");
			}

			ViewBag.Pagination = getPagination(model.Offset, model.TotalPosts, model.PageSize, forumId);

			model.CurrentUserId = Request.GetLoggedInUserId();
			return View("Items", model);
		}

		private PaginationModel getPagination(int offset, int totalPosts, int pageSize, Guid forumId)
		{
			return new PaginationModel
				{
					ActionName = "Items",
					ControllerName = "Moderation",
					Offset = offset,
					TotalItems = totalPosts,
					PageSize = pageSize,
					WriteTFoot = true,
					WriteTable = true,
					WriteTr = true,
					Identifier = forumId
				};
		}
	}
}