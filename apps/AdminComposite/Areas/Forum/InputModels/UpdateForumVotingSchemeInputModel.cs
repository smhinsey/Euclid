using System;
using Euclid.Composites.Mvc.Models;
using ForumAgent;
using ForumAgent.Commands;

namespace AdminComposite.Areas.Forum.InputModels
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