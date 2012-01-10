using System;
using System.Web.Mvc;
using AdminComposite.Models;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	public class ProfanityFilterController : Controller
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

		public PartialViewResult NewStopWord()
		{
			return PartialView("_NewStopWord");
		}
	}
}