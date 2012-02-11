using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class UpdateForumVotingScheme : DefaultCommand
	{
		public Guid ForumIdentifier { get; set; }
		public bool NoVoting { get; set; }
		public bool UpDownVoting { get; set; }
	}
}