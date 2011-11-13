using System;
using Euclid.Common.Storage;

namespace ForumAgent.WriteModels
{
	public class DomainOrganizationUser : IModel
	{
		public virtual DateTime Created { get; set; }
		public virtual Guid Identifier { get; set; }
		public virtual DateTime Modified { get; set; }
		public virtual DateTime LastLogin { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string Email { get; set; }
		public virtual string Username { get; set; }
		public virtual string PasswordSalt { get; set; }
		public virtual string PasswordHash { get; set; }
		public virtual DomainOrganization Organization { get; set; }
	}
}