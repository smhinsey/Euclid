using System;
using Euclid.Framework.Cqrs;

namespace ProfileAgent.Commands
{
	public class UpdateUserProfile : DefaultCommand
	{
		public Guid UserIdentifier { get; set; }
		public string AvatarUrl { get; set; }
		public string Email { get; set; } 
	}
}