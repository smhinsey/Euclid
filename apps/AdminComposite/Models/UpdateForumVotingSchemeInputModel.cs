using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent;
using ForumAgent.Commands;

namespace AdminComposite.Models
{
	public class UpdateForumVotingSchemeInputModel : DefaultInputModel
	{
		public UpdateForumVotingSchemeInputModel()
		{
			CommandType = typeof (UpdateForumVotingScheme);
		}

		public Guid ForumIdentifier { get; set; }
		public VotingScheme SelectedScheme { get; set; }
	}
}