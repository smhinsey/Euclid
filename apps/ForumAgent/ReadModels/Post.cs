using System;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class Post : DefaultReadModel
	{
		public virtual string AuthorDisplayName { get; set; }

		public virtual Guid AuthorIdentifier { get; set; }

		public virtual string Body { get; set; }

		public virtual Guid CategoryIdentifier { get; set; }

		public virtual string CategoryName { get; set; }

		public virtual string CategorySlug { get; set; }

		public virtual int CommentCount { get; set; }

		public virtual Guid ForumIdentifier { get; set; }

		public virtual int Score { get; set; }

		public virtual string Slug { get; set; }

		public virtual string Title { get; set; }
		
		public virtual string Tags { get; set; }

		public virtual int TotalVotes { get; set; }
	}
}