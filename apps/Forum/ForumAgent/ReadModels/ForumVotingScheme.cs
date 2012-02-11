using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class ForumVotingScheme : SyntheticReadModel
	{
		public Guid ForumIdentifier { get; set; }
		public string ForumName { get; set; }
		public VotingScheme CurrentScheme { get; set; }
	}
}