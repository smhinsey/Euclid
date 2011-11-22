using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;
using ForumAgent.ReadModels;

namespace AdminComposite.Models
{
	public class ForumThemeInputModel : DefaultInputModel
	{
		public ForumThemeInputModel()
		{
			CommandType = typeof (SetForumTheme);
			AgentSystemName = CommandType.Assembly.GetAgentMetadata().SystemName;
		}

		public Guid ForumIdentifier { get; set; }

		public IList<ForumTheme> AvailableThemes { get; set; }

		public string SelectedTheme { get; set; }

		public string SelectedPreviewUrl { get; set; }
	}
}