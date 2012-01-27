using System;
using System.Collections.Generic;
using Euclid.Common.Storage;
using ForumAgent.ReadModels;

namespace ForumAgent.Domain.Entities
{
	public class OrganizationUserEntity : IModel
	{
		public virtual DateTime Created { get; set; }

		public virtual Guid CreatedBy { get; set; }

		public virtual string Email { get; set; }

		public virtual string FirstName { get; set; }

		public virtual Guid Identifier { get; set; }

		public virtual DateTime LastLogin { get; set; }

		public virtual string LastName { get; set; }

		public virtual DateTime Modified { get; set; }

		public virtual OrganizationEntity OrganizationEntity { get; set; }

		public virtual string PasswordHash { get; set; }

		public virtual string PasswordSalt { get; set; }

		public virtual string Username { get; set; }

		public virtual bool Active { get; set; }

		public virtual IList<Forum> Forums { get; set; }
	}
}