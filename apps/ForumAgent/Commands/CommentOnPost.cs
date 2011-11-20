using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CommentOnPost : DefaultCommand
	{
		public Guid AuthorIdentifier { get; set; }

		public string Body { get; set; }

		public Guid ForumIdentifier { get; set; }

		public Guid PostIdentifier { get; set; }

		public string Title { get; set; }

		public bool ModerationRequired { get; set; }
	}
}