using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class ActivateBadge : DefaultCommand
	{
		public Guid BadgeIdentifier { get; set; }
		public bool Active { get; set; }
	}
}