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
		private const string ThemePathFormat = "~/Content/chromatron/img/forum-themes/{0}.png";

		public ThemeController(ForumQueries forumQueries, IPublisher publisher)
		{
			_forumQueries = forumQueries;
			_publisher = publisher;
		}

		public ActionResult List(Guid forumId)
		{
			var forum = _forumQueries.FindById(forumId);
			ViewBag.ForumName = forum.Name;

			var model = new SetForumThemeInputModel
							{
								ForumIdentifier = forumId,
								CurrentTheme = new Tuple<string, string>(forum.Theme, Url.Content(string.Format(ThemePathFormat, forum.Theme))),
								AvailableThemes = new List<Tuple<string, string>>
								                  	{
								                  		new Tuple<string, string>("Swiss", Url.Content(string.Format(ThemePathFormat, "swiss"))),
														new Tuple<string, string>("Test-Theme1", Url.Content(string.Format(ThemePathFormat, "test-theme1"))),
														new Tuple<string, string>("Test-Theme2", Url.Content(string.Format(ThemePathFormat, "test-theme2")))
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