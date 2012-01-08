using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class CreateTag : DefaultCommand
	{
		public Guid ForumIdentifier { get; set; }
		public bool Active { get; set; }
		public string Name { get; set; }
	}
}