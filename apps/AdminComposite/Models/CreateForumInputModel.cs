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

		public string Description { get; set; }

		public string Name { get; set; }

		public Guid OrganizationId { get; set; }

		public string UrlHostName { get; set; }

		public string UrlSlug { get; set; }
	}
}