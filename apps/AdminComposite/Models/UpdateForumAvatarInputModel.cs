using System;
using System.Web;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class UpdateForumAvatarInputModel : DefaultInputModel
	{
		public UpdateForumAvatarInputModel()
		{
			CommandType = typeof (UpdateAvatar);
			AgentSystemName = CommandType.Assembly.GetAgentMetadata().SystemName;
		}

		public Guid AvatarIdentifier { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public HttpPostedFileBase Image { get; set; }
		public string ImageUrl { get; set; }
	}
}