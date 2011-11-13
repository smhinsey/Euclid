using System;
using System.Web.Mvc;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class ContentController : Controller
	{
		public ActionResult ForForum(Guid forumId)
		{
			return View();
		}

		public PartialViewResult NewContent(Guid forumId)
		{
			return PartialView();
		}

		public PartialViewResult TypeSpecificInput(string contentType)
		{
			PartialViewResult result;
			switch(contentType.ToLower())
			{
				case "richtext":
					result = PartialView("_wysiwg");
					break;
				default:
					result = PartialView("_default");
					break;
			}

			return result;
		}
	}
}