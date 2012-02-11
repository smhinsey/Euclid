using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class Category : DefaultReadModel
	{
		public virtual string Name { get; set; }
		public virtual string Slug { get; set; }
		public virtual int TotalPosts { get; set; }
		public virtual Guid ForumIdentifier { get; set; }
		public virtual bool Active { get; set; }
		public virtual Guid CreatedBy { get; set; }
	}
}