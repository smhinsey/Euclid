using System;
using System.Linq;
using System.Web.Mvc;
using AdminComposite.Areas.Forum.InputModels;
using AdminComposite.Controllers;
using ForumAgent.Queries;

namespace AdminComposite.Areas.Forum.Controllers
{
	[Authorize]
	public class ThemeController : AdminController
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
			var forum = _forumQueries.FindById(forumId);

			var forumThemes = _themeQueries.GetForumThemes(forumId);
			var currentTheme = forumThemes.Where(t => t.IsCurrent).FirstOrDefault();

			return
				View(
					new ForumThemeInputModel
						{
							ForumIdentifier = forumId,
							AvailableThemes = forumThemes,
							ForumName = forum.Name,
							SelectedTheme = (currentTheme == null) ? string.Empty : currentTheme.Name,
							SelectedPreviewUrl = (currentTheme == null) ? string.Empty : currentTheme.PreviewUrl
						});
		}
	}
}