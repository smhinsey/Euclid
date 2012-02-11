using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class DeleteAvatar : DefaultCommand
	{
		public Guid AvatarIdentifier { get; set; }
	}
}