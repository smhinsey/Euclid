using System;
using Euclid.Framework;

namespace ProfileAgent.ReadModel
{
	public class UserProfile : DefaultReadModel
	{
		public virtual Guid UserIdentifier { get; set; }
		public virtual string AvatarUrl { get; set; }
		public virtual string Email { get; set; } 
	}
}