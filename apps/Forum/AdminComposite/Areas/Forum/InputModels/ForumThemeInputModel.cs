using System;
using System.Collections.Generic;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace AdminComposite.Areas.Forum.InputModels
{
	public class ForumThemeInputModel : DefaultInputModel
	{
		public ForumThemeInputModel()
		{
			CommandType = typeof (SetForumTheme);
		}

		public Guid ForumIdentifier { get; set; }

		public IList<ForumTheme> AvailableThemes { get; set; }

		public string SelectedTheme { get; set; }

		public string SelectedPreviewUrl { get; set; }

		public string ForumName { get; set; }
	}
}