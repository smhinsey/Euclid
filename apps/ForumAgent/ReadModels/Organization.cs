using System;
using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class Organization : DefaultReadModel
	{
		public virtual string Name { get; set; }

		public virtual IList<Forum> Forums { get; set; }

		//public virtual IList<Guid> ContactIdentifiers { get; set; }

		//public virtual IList<Guid> OrgnizationAdminIdentifiers { get; set; }
	}
}