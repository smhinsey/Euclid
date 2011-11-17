using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CreateForumContent : DefaultCommand
	{
		public Guid ForumIdentifier { get; set; }
		public string Location { get; set; }
		public string Type { get; set; }
		public bool Active { get; set; }
		public string Value { get; set; }
	}
}