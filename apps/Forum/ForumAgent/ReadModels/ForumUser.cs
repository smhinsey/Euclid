using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class ForumUser : DefaultReadModel
	{
		public virtual string Email { get; set; }

		public virtual string FirstName { get; set; }

		public virtual Guid ForumIdentifier { get; set; }

		public virtual string LastName { get; set; }

		public virtual string PasswordHash { get; set; }

		public virtual string PasswordSalt { get; set; }

		public virtual string Username { get; set; }

		public virtual bool IsBlocked { get; set; }

		public virtual DateTime LastLogin { get; set; }

		public virtual Guid CreatedBy { get; set; }

		public virtual int PostCount { get; set; }
		public virtual int PointTotal { get; set; }
		public virtual int BadgeCount { get; set; }

		public virtual int CommentCount { get; set; }

		public virtual int NumberVotes { get; set; }

		public virtual bool Active { get; set; }
	}
}