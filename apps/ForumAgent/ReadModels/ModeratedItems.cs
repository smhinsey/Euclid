using System;
using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class ModeratedItems : SyntheticReadModel
	{
		public IList<dynamic> Posts { get; set; }

		public int TotalPosts { get; set; }

		public int Offset { get; set; }

		public int PageSize { get; set; }

		public Guid ForumIdentifier { get; set; }

		public string ForumName { get; set; }

		public Guid CurrentUserId { get; set; }
	}
}