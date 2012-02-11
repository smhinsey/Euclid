using System;
using System.Web.Mvc;
using AdminComposite.Areas.Forum.InputModels;
using AdminComposite.Controllers;
using ForumAgent.Queries;

namespace AdminComposite.Areas.Forum.Controllers
{
	public class ProfanityFilterController : AdminController
	{
		private readonly ProfanityFilterQueries _profanityFilterQueries;

		public ProfanityFilterController(ProfanityFilterQueries profanityFilterQueries)
		{
			_profanityFilterQueries = profanityFilterQueries;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			var model = _profanityFilterQueries.List(forumId, offset, pageSize);

			ViewBag.Pagination = new PaginationModel
				{
					ActionName = "List",
					ControllerName = "Content",
					Identifier = forumId,
					Offset = offset,
					PageSize = pageSize,
					TotalItems = model.TotalStopWords
				};

			return View(model);
		}

		public PartialViewResult NewStopWord(Guid forumId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			return PartialView("_NewStopWord", new CreateStopWordInputModel { ForumIdentifier = forumId, CreatedBy = userId, });
		}
	}
}