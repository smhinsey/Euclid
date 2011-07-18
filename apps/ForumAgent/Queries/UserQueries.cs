using Euclid.Common.Storage.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	// SELF long-term, we can't actually inherit directly from NhSimpleRepo here, we need something that filters out the save, update, and delete features
	public class UserQueries : NhSimpleRepository<User>
	{
		public UserQueries(ISession session) : base(session)
		{
		}

		public bool Authenticate(string username, string password)
		{
			// TODO: implement safe hashing/salting and all that noise

			var session = GetCurrentSession();

			var category = session.QueryOver<User>()
				.Where(x => x.PasswordHash == password)
				.Where(x => x.PasswordSalt == password)
				.Where(x => x.Username == username);

			return category != null;
		}
	}
}