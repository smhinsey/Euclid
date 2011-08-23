using System;
using Euclid.Framework.Cqrs;

namespace ForumAgent.Commands
{
	public class UpdateUserProfile : DefaultCommand
	{
		public string AvatarUrl { get; set; }

		public string Email { get; set; }

		public Guid UserIdentifier { get; set; }
	}
}