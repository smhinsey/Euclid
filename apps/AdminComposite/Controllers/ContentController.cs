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
		private readonly IPublisher _publisher;

		public ContentController(ContentQueries contentQueries, IPublisher publisher)
		{
			_contentQueries = contentQueries;
			_publisher = publisher;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			return View(_contentQueries.List(offset, pageSize));
		}

		public PartialViewResult NewContent(Guid forumId)
		{
			return PartialView("_NewContent", new CreateForumContentInputModel
			                                  	{
			                                  		ForumIdentifier = forumId,
													CreatedBy = ViewBag.UserId
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
			                                     		Value = content.Value
			                                     	});
		}

		public PartialViewResult TypeSpecificInput(AvailableContentType contentType, string value)
		{
			PartialViewResult result;
			switch (contentType)
			{
				case AvailableContentType.RichText:
					result = PartialView("_wysiwg", value);
					break;
				case AvailableContentType.EmeddedYouTube:
					result = PartialView("_default", value);
					break;
				default:
					throw new NotImplementedException("Invalid content type specified");
			}

			return result;
		}

		public JsonResult Delete(Guid contentId)
		{
			var publicationId = _publisher.PublishMessage(new DeleteForumContent {ContentIdentifier = contentId});

			return Json(publicationId, JsonRequestBehavior.AllowGet);
		}

		public PartialViewResult Preview(Guid contentId)
		{
			var content = _contentQueries.FindById(contentId);

			return PartialView("_preview", content);
		}
	}
}