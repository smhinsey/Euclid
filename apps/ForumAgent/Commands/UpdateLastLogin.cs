using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class UpdateLastLogin : DefaultCommand
	{
		public Guid UserIdentifier { get; set; }
		public DateTime LoginTime { get; set; }
	}
}