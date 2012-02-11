using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class PostListing : DefaultReadModel
	{
		public virtual IList<Post> Posts { get; set; }

		public virtual int TotalPosts { get; set; }
		public virtual string CategoryName { get; set; }
		public virtual string TagName { get; set; }
	}
}