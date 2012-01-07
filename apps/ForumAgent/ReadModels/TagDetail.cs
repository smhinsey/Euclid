using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class TagDetail : DefaultReadModel
	{
		public virtual Tag Tag { get; set; }
		public virtual IList<Post> Posts { get; set; }
	}
}