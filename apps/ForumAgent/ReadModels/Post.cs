﻿using System;
using Euclid.Framework;

namespace ForumAgent.ReadModels
{
	public class Post : DefaultReadModel
	{
		public virtual Guid AuthorIdentifier { get; set; }
		public virtual Guid CategoryIdentifier { get; set; }
		public virtual string Body { get; set; }
		public virtual int Score { get; set; }
		public virtual string Title { get; set; }
	}
}