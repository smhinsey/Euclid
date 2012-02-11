using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class UpdateCategory : DefaultCommand
	{
		public Guid CategoryIdentifier { get; set; }
		public bool Active { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
	}
}