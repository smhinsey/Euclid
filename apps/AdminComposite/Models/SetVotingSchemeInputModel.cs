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

		public string ForumName { get; set; }
		public Guid ForumIdentifier { get; set; }
		public VotingScheme SelectedScheme { get; set; }
	}

	public enum VotingScheme
	{
		NoVoting,
		UpDownVoting
	};
}