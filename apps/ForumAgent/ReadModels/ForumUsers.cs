using System.Collections.Generic;
using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class ForumUsers : SyntheticReadModel
	{
		private Guid ForumIdentifier { get; set; }
		public IList<ForumUser> Users { get; set; }
	}
}