using System;
using Euclid.Framework.Cqrs;

namespace PublicForm.ForumAgent.Commands
{
	public class PublishPost : DefaultCommand
	{
		public string Title { get; set; }
		public string Body { get; set; }
		public Guid AuthorIdentifier { get; set; }
	}
}