using System;
using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class OrganizationUsers: SyntheticReadModel
	{
		public string OrganizationName { get; set; }
		public string OrganizationSlug { get; set; }
		public Guid OrganizationIdentifier { get; set; }
		public IList<OrganizationUser> Users { get; set; }
		public int TotalNumberOfUsers { get; set; }
	}
}