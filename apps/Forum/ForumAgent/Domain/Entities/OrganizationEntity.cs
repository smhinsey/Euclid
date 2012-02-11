using System;
using System.Collections.Generic;
using Euclid.Common.Storage;

namespace ForumAgent.Domain.Entities
{
	public class OrganizationEntity : IModel
	{
		public OrganizationEntity()
		{
			Users = new List<OrganizationUserEntity>();
		}

		public virtual string Address { get; set; }

		public virtual string Address2 { get; set; }

		public virtual string City { get; set; }

		public virtual string Country { get; set; }

		public virtual DateTime Created { get; set; }

		public virtual Guid CreatedBy { get; set; }

		public virtual Guid Identifier { get; set; }

		public virtual DateTime Modified { get; set; }

		public virtual string OrganizationName { get; set; }

		public virtual string OrganizationSlug { get; set; }

		public virtual string OrganizationUrl { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual string State { get; set; }

		public virtual IList<OrganizationUserEntity> Users { get; protected set; }

		public virtual string Zip { get; set; }
	}
}