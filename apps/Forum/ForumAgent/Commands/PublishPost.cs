using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class PublishPost : DefaultCommand
	{
		public Guid AuthorIdentifier { get; set; }

		public string Body { get; set; }

		public Guid CategoryIdentifier { get; set; }

		public Guid ForumIdentifier { get; set; }

		public string Title { get; set; }
		
		public string[] Tags { get; set; }

		public bool ModerationRequired { get; set; }
	}
}