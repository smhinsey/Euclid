using System;
using Euclid.Framework;

namespace ForumAgent.ReadModels
{
	public class Post : DefaultReadModel
	{
		public virtual string Title { get; set; }
		public virtual string Body { get; set; }
		public virtual Guid AuthorIdentifier { get; set; }
		public virtual int Score { get; set; }
	}
}