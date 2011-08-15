using System;
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

			var matchedAccount = session.QueryOver<User>()
				.Where(user => user.PasswordHash == password)
				.Where(user => user.PasswordSalt == password)
				.Where(user => user.Username == username);

			return matchedAccount != null;
		}

		public User FindByUsername(string username)
		{
			
			var session = GetCurrentSession();

			var matchedUser = session.QueryOver<User>()
				.Where(user => user.Username == username);

			return matchedUser.SingleOrDefault();
		}

		public UserProfile FindByUserIdentifier(Guid identifier)
		{

			var session = GetCurrentSession();

			var matchedUser = session.QueryOver<UserProfile>()
				.Where(user => user.UserIdentifier == identifier);

			return matchedUser.SingleOrDefault();
		}
	}
}