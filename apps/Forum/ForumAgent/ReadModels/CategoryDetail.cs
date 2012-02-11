using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class CategoryDetail : DefaultReadModel
	{
		public virtual Category Category { get; set; }
		public virtual IList<Post> Posts { get; set; }
	}
}