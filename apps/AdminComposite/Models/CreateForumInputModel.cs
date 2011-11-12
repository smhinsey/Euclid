using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class CreateForumInputModel : DefaultInputModel
	{
		public CreateForumInputModel()
		{
			AgentSystemName = "NewCo.ForumAgent";
			CommandType = typeof(CreateForum);
		}

		public string Name { get; set; }

		public string UrlHostName { get; set; }

		public string UrlSlug { get; set; }

		public string Description { get; set; }

		public Guid OrganizationId { get; set; }
	}
}