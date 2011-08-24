using Euclid.Framework.Models;

namespace ForumAgent.ReadModels
{
	public class User : DefaultReadModel
	{
		public virtual string PasswordHash { get; set; }

		public virtual string PasswordSalt { get; set; }

		public virtual string Username { get; set; }
	}
}
