using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CommentOnPost : DefaultCommand
	{
		public string Title { get; set; }
		public string Body { get; set; }
		public Guid AuthorIdentifier { get; set; }
		public Guid PostIdentifier { get; set; }
	}
}