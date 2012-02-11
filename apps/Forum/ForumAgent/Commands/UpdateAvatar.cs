using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class UpdateAvatar : DefaultCommand
	{
		public Guid AvatarIdentifier { get; set; }
		public string Name { get; set; }
		public string ImageUrl { get; set; }
		public string Description { get; set; }
	}
}