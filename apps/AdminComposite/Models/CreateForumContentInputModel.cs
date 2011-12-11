using System;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class CreateForumContentInputModel : DefaultInputModel
	{
		public CreateForumContentInputModel()
		{
			CommandType = typeof (CreateForumContent);
		}

		public Guid ForumIdentifier { get; set; }
		public Guid CreatedBy { get; set; }
		public bool Active { get; set; }
		public string Location { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }
	}
}