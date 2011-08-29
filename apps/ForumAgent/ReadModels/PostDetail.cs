using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class PostDetail : DefaultReadModel
	{
		public virtual Post InitialPost { get; set; }

		public virtual IList<Comment> Comments { get; set; }
	}
}