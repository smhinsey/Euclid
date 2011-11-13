using System;
using System.Collections.Generic;
using Euclid.Common.Storage;

namespace ForumAgent.WriteModels
{
	public class DomainOrganization : IModel
	{
		public virtual DateTime Created { get; set; }
		public virtual Guid Identifier { get; set; }
		public virtual DateTime Modified { get; set; }
		public virtual string OrganizationName { get; set; }
		public virtual string OrganizationUrl { get; set; }
		public virtual string PhoneNumber { get; set; }
		public virtual string Address { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string Zip { get; set; }
		public virtual string Country { get; set; }

		public virtual IList<DomainOrganizationUser> Users { get; protected set; }

		public DomainOrganization()
		{
			Users = new List<DomainOrganizationUser>();
		}
	}
}