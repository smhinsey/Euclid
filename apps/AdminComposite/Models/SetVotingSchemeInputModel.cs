using System;
using Euclid.Composites.Mvc.Models;
using Euclid.Framework.AgentMetadata.Extensions;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class SetVotingSchemeInputModel : DefaultInputModel
	{
		public enum AvailableScheme
		{
			NoVoting,
			UpDownVoting
		};

		public SetVotingSchemeInputModel()
		{
			CommandType = typeof (UpdateForumVotingScheme);
			AgentSystemName = CommandType.Assembly.GetAgentMetadata().SystemName;
		}

		public Guid ForumIdentifier { get; set; }
		public AvailableScheme SelectedScheme { get; set; }
	}
}