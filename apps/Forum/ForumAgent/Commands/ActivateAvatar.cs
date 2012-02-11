using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class ActivateAvatar : DefaultCommand
	{
		public Guid AvatarIdentifier { get; set; }
		public bool Active { get; set; }
	}
}