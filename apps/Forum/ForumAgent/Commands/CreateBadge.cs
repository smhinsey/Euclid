using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CreateBadge : DefaultCommand
	{
		public Guid ForumIdentifier { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public string Field { get; set; }
		public string Operator { get; set; }
		public string Value { get; set; }
	}
}