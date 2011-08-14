using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class UserQueries : NhQuery<User>
	{
		public UserQueries(ISession session) : base(session)
		{
		}

		public bool Authenticate(string username, string password)
		{
			// TODO: implement safe hashing/salting and all that noise

			var session = GetCurrentSession();

			var category = session.QueryOver<User>()
				.Where(user => user.PasswordHash == password)
				.Where(user => user.PasswordSalt == password)
				.Where(user => user.Username == username);

			return category != null;
		}
	}
}