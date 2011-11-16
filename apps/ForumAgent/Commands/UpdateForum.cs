using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class UpdateForum : DefaultCommand
	{
		public Guid ForumIdentifier { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public string Description { get; set; }
	}
}