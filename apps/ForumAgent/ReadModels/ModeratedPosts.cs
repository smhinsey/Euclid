using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class ModeratedPosts : UnpersistedReadModel
	{
		public IList<ModeratedPost> Posts { get; set; }

		public int TotalPosts { get; set; }

		public int Offset { get; set; }

		public int PageSize { get; set; }
	}
}