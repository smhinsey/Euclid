using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class ActivateForumUser: DefaultCommand
	{
		public Guid UserIdentifier { get; set; }
		public bool Active { get; set; }
	}
}