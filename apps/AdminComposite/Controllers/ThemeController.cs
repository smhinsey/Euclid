using System;
using System.Linq;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class ThemeController : Controller
	{
		private readonly ForumQueries _forumQueries;
		private readonly ThemeQueries _themeQueries;
		
		public ThemeController(ForumQueries forumQueries, ThemeQueries themeQueries)
		{
			_forumQueries = forumQueries;
			_themeQueries = themeQueries;
		}

		public ActionResult List(Guid forumId)
		{
			Forum forum = _forumQueries.FindById(forumId);
			ViewBag.ForumName = forum.Name;

			var availableThemes = _themeQueries.GetForumThemes(forumId);
			var currentTheme = availableThemes.Where(t => t.IsCurrent).FirstOrDefault();

			return View(new ForumThemeInputModel
			            	{
			            		ForumIdentifier = forumId,
								AvailableThemes = availableThemes,
								SelectedTheme = (currentTheme == null) ? string.Empty : currentTheme.Name,
								SelectedPreviewUrl = (currentTheme == null) ? string.Empty : currentTheme.PreviewUrl
			            	});
		}
	}
}