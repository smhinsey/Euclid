using System;
using System.Collections.Generic;
using Euclid.Composites;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class Organization : UnpersistedReadModel
	{
		public virtual string Name { get; set; }
		public virtual string WebsiteUrl { get; set; }
		public virtual string PhoneNumber { get; set; }
		public virtual string Address { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string City { get; set; }
		public virtual string State { get; set; }
		public virtual string Zip { get; set; }
		public virtual string Country { get; set; }
		
	}
}