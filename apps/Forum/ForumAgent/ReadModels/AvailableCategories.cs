using System;
using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class AvailableCategories : SyntheticReadModel
	{
		public string ForumName { get; set; }
		public Guid ForumIdentifier { get; set; }
		public IList<Category> Categories { get; set; }
		public int TotalCategories { get; set; }
	}
}