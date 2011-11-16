using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class UpdateForumInputModel : DefaultInputModel
	{
		public UpdateForumInputModel()
		{
			AgentSystemName = "NewCo.ForumAgent";
			CommandType = typeof(UpdateForum);
		}

		public Guid ForumIdentifier { get; set; }

		public string Name { get; set; }

		public string UrlHostName { get; set; }

		public string UrlSlug { get; set; }

		public string Description { get; set; }
	}
}