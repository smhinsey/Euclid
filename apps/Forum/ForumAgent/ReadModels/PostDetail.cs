using System.Collections.Generic;
using Euclid.Framework.Models;
using NHibernate;

namespace ForumAgent.ReadModels
{
	public class PostDetail : DefaultReadModel
	{
		public virtual IList<Comment> Comments { get; set; }

		public virtual Post InitialPost { get; set; }

		public virtual Category Category { get; set; }
	}
}