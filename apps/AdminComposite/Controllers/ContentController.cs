using System;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	public enum AvailableContentType { RichText, EmeddedYouTube }

	[Authorize]
	public class ContentController : Controller
	{
		private readonly ContentQueries _contentQueries;
		private readonly ForumQueries _forumQueries;
		private readonly IPublisher _publisher;

		public ContentController(ContentQueries contentQueries, ForumQueries forumQueries, IPublisher publisher)
		{
			_contentQueries = contentQueries;
			_forumQueries = forumQueries;
			_publisher = publisher;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			var model = _contentQueries.List(forumId, offset, pageSize);
			ViewBag.ForumName = model.ForumName;
			ViewBag.Pagination = new PaginationModel
			                     	{
			                     		ActionName = "List",
			                     		ControllerName = "Content",
			                     		ForumIdentifier = forumId,
			                     		Offset = offset,
			                     		PageSize = pageSize,
			                     		TotalItems = model.TotalContentItems
			                     	};

			return View(model);
		}

		public PartialViewResult NewContent(Guid forumId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			return PartialView("_NewContent", new CreateForumContentInputModel
			                                  	{
			                                  		ForumIdentifier = forumId,
													CreatedBy = userId,
			                                  	});
		}

		public PartialViewResult UpdateContent(Guid contentId)
		{
			var content = _contentQueries.FindById(contentId);

			return PartialView("_UpdateContent", new UpdateForumContentInputModel
			                                     	{
														ForumIdentifier =  content.ForumIdentifier,
			                                     		ContentIdentifier = contentId,
			                                     		Active = content.Active,
			                                     		Location = content.ContentLocation,
			                                     		Type = content.ContentType,
			                                     		Value = content.Value,
														PartialView = GetPartialViewNameForContentType(content.ContentType)
			                                     	});
		}

		public PartialViewResult TypeSpecificInput(AvailableContentType contentType, string value)
		{
			var name = GetPartialViewNameForContentType(contentType);

			return PartialView(name, value);
		}

		public JsonResult Delete(Guid contentId)
		{
			var publicationId = _publisher.PublishMessage(new DeleteForumContent {ContentIdentifier = contentId});

			return Json(publicationId, JsonRequestBehavior.AllowGet);
		}

		public JsonResult ActivateContent(Guid contentId, bool active)
		{
			var publicationId = _publisher.PublishMessage(new ActivateContent {ContentIdentifier = contentId, Active = active});

			return Json(publicationId, JsonRequestBehavior.AllowGet);
		}

		public PartialViewResult Preview(Guid contentId)
		{
			var content = _contentQueries.FindById(contentId);

			return PartialView("_preview", content);
		}



		private string GetPartialViewNameForContentType(string contentType)
		{
			return GetPartialViewNameForContentType((AvailableContentType)Enum.Parse(typeof (AvailableContentType), contentType));
		}

		private string GetPartialViewNameForContentType(AvailableContentType contentType)
		{
			string viewName;
			switch (contentType)
			{
				case AvailableContentType.RichText:
					viewName = "_wysiwg";
					break;
				case AvailableContentType.EmeddedYouTube:
					viewName = "_youtube";
					break;
				default:
					throw new NotImplementedException("Invalid content type specified");
			}

			return viewName;
		}
	}
}