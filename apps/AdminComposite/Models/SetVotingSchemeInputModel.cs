using System;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class SetVotingSchemeInputModel : DefaultInputModel
	{
		public SetVotingSchemeInputModel()
		{
			CommandType = typeof (UpdateForumVotingScheme);
			AgentSystemName = CommandType.Assembly.GetAgentMetadata().SystemName;
		}

		public Guid ForumIdentifier { get; set; }
		public bool NoVoting { get; set; }
		public bool UpDownVoting { get; set; }
	}
}