using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class MarkCommentAsFavorite : DefaultCommand
	{
		public Guid ForumIdentifier { get; set; }
		public Guid PostIdentifier { get; set; }
		public Guid CommentIdentifier { get; set; }
		public Guid UserIdentifier { get; set; }
	}
}