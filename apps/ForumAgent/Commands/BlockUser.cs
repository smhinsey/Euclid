using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class BlockUser : DefaultCommand
	{
		public Guid UserIdentifier { get; set; }
	}

	public class UnblockUser : DefaultCommand
	{
		public Guid UserIdentifier { get; set; }
	}
}