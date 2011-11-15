using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class Organization : UnpersistedReadModel
	{
		public virtual string Name { get; set; }

		public virtual string Slug { get; set; }
	}
}