using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class VoteOnComment : DefaultCommand
	{
		public Guid AuthorIdentifier { get; set; }

		public Guid CommentIdentifier { get; set; }

		public bool VoteUp { get; set; }
	}
}