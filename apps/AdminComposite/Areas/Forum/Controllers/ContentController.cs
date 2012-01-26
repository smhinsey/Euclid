using System;
using System.Web.Mvc;
using AdminComposite.Areas.Forum.InputModels;
using AdminComposite.Controllers;
using ForumAgent.Queries;

namespace AdminComposite.Areas.Forum.Controllers
{
	[Authorize]
	public class ContentController : AdminController
	{
		private readonly ContentQueries _contentQueries;

		public ContentController(ContentQueries contentQueries)
		{
			_contentQueries = contentQueries;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			var model = _contentQueries.List(forumId, offset, pageSize);
			ViewBag.Pagination = new PaginationModel
				{
					ActionName = "List",
					ControllerName = "Content",
					Identifier = forumId,
					Offset = offset,
					PageSize = pageSize,
					TotalItems = model.TotalContentItems
				};

			return View(model);
		}

		public PartialViewResult NewContent(Guid forumId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			return PartialView(
				"_NewContent", new CreateForumContentInputModel { ForumIdentifier = forumId, CreatedBy = userId, });
		}

		public PartialViewResult Preview(Guid contentId)
		{
			var content = _contentQueries.FindById(contentId);

			return PartialView("_preview", content);
		}

		public PartialViewResult TypeSpecificInput(AvailableContentType contentType, string value)
		{
			var name = getPartialViewNameForContentType(contentType);

			return PartialView(name, value);
		}

		public PartialViewResult UpdateContent(Guid contentId)
		{
			var content = _contentQueries.FindById(contentId);

			return PartialView(
				"_UpdateContent",
				new UpdateForumContentInputModel
					{
						ForumIdentifier = content.ForumIdentifier,
						ContentIdentifier = contentId,
						Active = content.Active,
						Location = content.ContentLocation,
						Type = content.ContentType,
						Value = content.Value,
						PartialView = getPartialViewNameForContentType(content.ContentType)
					});
		}

		private static string getPartialViewNameForContentType(string contentType)
		{
			return getPartialViewNameForContentType((AvailableContentType)Enum.Parse(typeof(AvailableContentType), contentType));
		}

		private static string getPartialViewNameForContentType(AvailableContentType contentType)
		{
			string viewName;
			switch (contentType)
			{
				case AvailableContentType.RichText:
					viewName = "_wysiwg";
					break;
				case AvailableContentType.EmbeddedYouTube:
					viewName = "_youtube";
					break;
				default:
					throw new NotImplementedException("Invalid content type specified");
			}

			return viewName;
		}
	}
}