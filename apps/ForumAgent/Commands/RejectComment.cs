using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class RejectComment : DefaultCommand
	{
		public Guid CommentIdentifier { get; set; }
	}
}