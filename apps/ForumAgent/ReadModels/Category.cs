using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class Category : DefaultReadModel
	{
		public virtual int CommentCount { get; set; }
		public virtual string Name { get; set; }
		public virtual Guid ParentCategoryIdentifier { get; set; }
		public virtual int PostCount { get; set; }
	}
}