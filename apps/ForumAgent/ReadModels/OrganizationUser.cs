using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class OrganizationUser : UnpersistedReadModel
	{
		public virtual Guid OrganizationIdentifier { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string Email { get; set; }
		public virtual string Username { get; set; }
		public virtual DateTime LastLogin { get; set; }
		public virtual string PasswordHash { get; set; }
		public virtual string PasswordSalt { get; set; }
	}
}