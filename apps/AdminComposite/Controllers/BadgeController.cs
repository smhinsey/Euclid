using System;
using System.Web.Mvc;
using AdminComposite.Models;
using ForumAgent.Commands;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class BadgeController : Controller
	{
		private readonly BadgeQueries _badgeQueries;

		public BadgeController(BadgeQueries badgeQueries)
		{
			_badgeQueries = badgeQueries;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			var badges = _badgeQueries.FindBadges(forumId, offset, pageSize);
			ViewBag.Pagination = new PaginationModel
									{
										ActionName = "List",
										ControllerName = "Badge",
										Identifier = forumId,
										TotalItems = badges.TotalBadges,
										Offset = offset,
										PageSize = pageSize,
										WriteTable = false,
										WriteTFoot = false,
										WriteTr = false
									};

			return View(badges);
		}

		public PartialViewResult NewBadge(Guid forumId)
		{
			return PartialView("_NewBadge", new CreateBadgeInputModel
			                                	{
			                                		ForumIdentifier = forumId,
			                                	});
		}

		public PartialViewResult UpdateBadge(Guid badgeId)
		{
			var badge = _badgeQueries.FindById(badgeId);

			return PartialView("_UpdateBadge", new UpdateBadgeInputModel
			                                   	{
			                                   		BadgeIdentifier = badgeId,
													Description = badge.Description,
													Field = badge.Field,
													ImageUrl = badge.ImageUrl,
													Name = badge.Name,
													Operator = badge.Operator,
													Value = badge.Value
			                                   	});
		}
	}
}