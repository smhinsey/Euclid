using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class CreateForumInputModel : DefaultInputModel
	{
		public CreateForumInputModel()
		{
			CommandType = typeof(CreateForum);
		}

		public string Description { get; set; }

		public string Name { get; set; }

		public Guid OrganizationId { get; set; }

		public string UrlHostName { get; set; }

		public string UrlSlug { get; set; }

		public VotingScheme VotingScheme { get; set; }

		public Guid CreatedBy { get; set; }

		public string Theme { get; set; }

		public bool Moderated { get; set; }

		public bool Private { get; set; }
	}
}