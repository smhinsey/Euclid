using System;
using System.Web;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class CreateForumAvatarInputModel : DefaultInputModel
	{
		public CreateForumAvatarInputModel()
		{
			CommandType = typeof (CreateAvatar);
			AgentSystemName = CommandType.Assembly.GetAgentMetadata().SystemName;
		}

		public Guid CreatedBy { get; set; }
		public Guid ForumIdentifier { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public HttpPostedFileBase Image { get; set; }
	}
}