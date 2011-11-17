using System;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class UpdateForumContentInputModel : DefaultInputModel
	{
		public  UpdateForumContentInputModel()
		{
			CommandType = typeof (UpdateForumContent);
			AgentSystemName = CommandType.Assembly.GetAgentMetadata().SystemName;
		}

		public Guid ForumIdentifier { get; set; }
		public Guid ContentIdentifier { get; set; }
		public bool Active { get; set; }
		public string Location { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }
		public string PartialView { get; set; }
	}
}