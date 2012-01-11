using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class ForumUserListing : DefaultReadModel
	{
		public virtual IList<ForumUser> Users { get; set; }

		public virtual int TotalUsers { get; set; }
	}
}