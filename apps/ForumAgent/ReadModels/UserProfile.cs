using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class UserProfile : DefaultReadModel
	{
		public virtual string AvatarUrl { get; set; }

		public virtual string Email { get; set; }

		public virtual Guid UserIdentifier { get; set; }
	}
}