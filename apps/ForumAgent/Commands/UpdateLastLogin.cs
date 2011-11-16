using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class UpdateLastLogin : DefaultCommand
	{
		public DateTime LoginTime { get; set; }

		public Guid UserIdentifier { get; set; }
	}
}