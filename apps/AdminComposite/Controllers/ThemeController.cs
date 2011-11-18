using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class ThemeController : Controller
	{
		private readonly ForumQueries _forumQueries;
		private readonly IPublisher _publisher;

		public ThemeController(ForumQueries forumQueries, IPublisher publisher)
		{
			_forumQueries = forumQueries;
			_publisher = publisher;
		}

		public ActionResult List(Guid forumId)
		{
			var forum = _forumQueries.FindById(forumId);

			var model = new SetForumThemeInputModel
							{
								ForumIdentifier = forumId,
								CurrentTheme = new Tuple<string, string>("Swiss", Url.Content("~/Content/swiss.png")),
								AvailableThemes = new List<Tuple<string, string>>
								                  	{
								                  		new Tuple<string, string>("Swiss", Url.Content("~/Content/Swiss.png")),
														new Tuple<string, string>("Test-Theme1", Url.Content("~/Content/chromatron/img/sample_logo.png")),
														new Tuple<string, string>("Test-Theme2", Url.Content("~/Content/chromatron/img/sample_logo2.png")),
								                  	}
							};
			return View(model);
		}

		public JsonResult SetForumTheme(Guid forumId, string theme)
		{
			var publicationId = _publisher.PublishMessage(new SetForumTheme
			{
				ForumIdentifier = forumId,
				ThemeName = theme
			});

			return Json(new { publicationId }, JsonRequestBehavior.AllowGet);
		}
	}
}