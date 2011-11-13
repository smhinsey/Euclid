using System;
using System.Collections.Generic;
using Euclid.Composites;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class Organization : UnpersistedReadModel
	{
		public virtual string Name { get; set; }

		public virtual IList<Forum> Forums { get; set; }
	}
}