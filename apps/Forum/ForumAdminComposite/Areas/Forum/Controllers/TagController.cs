using System;
using System.Web.Mvc;
using AdminComposite.Areas.Forum.InputModels;
using AdminComposite.Controllers;
using ForumAgent;
using ForumAgent.Queries;

namespace AdminComposite.Areas.Forum.Controllers
{
	[Authorize]
	public class TagController : AdminController
	{
		private readonly TagQueries _tagQueries;

		public TagController(TagQueries tagQueries)
		{
			_tagQueries = tagQueries;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			var model = _tagQueries.List(forumId, offset, pageSize);

			ViewBag.Pagination = new PaginationModel
				{
					ActionName = "List",
					ControllerName = "Category",
					Identifier = forumId,
					PageSize = pageSize,
					Offset = offset,
					TotalItems = model.TotalTags
				};

			return View(model);
		}

		public PartialViewResult NewTag(Guid forumId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			return PartialView("_NewTag", new CreateTagInputModel { ForumIdentifier = forumId, CreatedBy = userId });
		}

		public PartialViewResult UpdateTag(Guid tagId)
		{
			var tag = _tagQueries.FindById(tagId);

			if (tag == null)
			{
				throw new CategoryNotFoundException(string.Format("Could not find a tag with id {0}", tagId));
			}

			var model = new UpdateTagInputModel { TagIdentifier = tagId, Name = tag.Name, Active = tag.Active };

			return PartialView("_UpdateTag", model);
		}
	}
}