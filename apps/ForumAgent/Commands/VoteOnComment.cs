using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class VoteOnComment : DefaultCommand
	{
		public Guid AuthorIdentifier { get; set; }
		public Guid PostIdentifier { get; set; }
		public bool VoteUp { get; set; } 
	}
}