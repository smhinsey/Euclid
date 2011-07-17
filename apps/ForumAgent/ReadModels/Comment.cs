﻿using System;
using Euclid.Framework;

namespace PublicForm.ForumAgent.ReadModels
{
	public class Comment : DefaultReadModel
	{
		public virtual string Body { get; set; }
		public virtual Guid AuthorIdentifier { get; set; }
		public virtual Guid PostIdentifier { get; set; }
		public virtual int Score { get; set; }
	}
}