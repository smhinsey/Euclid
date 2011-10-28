using System;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class UserQueries : NhQuery<User>
	{
		public UserQueries(ISession session)
			: base(session)
		{
		}

		public bool Authenticate(string username, string password)
		{
			// TODO: implement safe hashing/salting and all that noise
			var session = GetCurrentSession();

			var matchedAccount =
				session.QueryOver<User>().Where(user => user.PasswordHash == password && user.PasswordSalt == password && user.Username == username);

			return matchedAccount != null;
		}

		public UserProfile FindByUserIdentifier(Guid identifier)
		{
			var session = GetCurrentSession();

			var matchedUser = session.QueryOver<UserProfile>().Where(user => user.UserIdentifier == identifier);

			return matchedUser.SingleOrDefault();
		}

		public User FindByUsername(string username)
		{
			var session = GetCurrentSession();

			var matchedUser = session.QueryOver<User>().Where(user => user.Username == username);

			return matchedUser.SingleOrDefault();
		}

		public UserProfile UserProfileByUsername(string username)
		{
			var matchedUser = FindByUsername(username);

			return FindByUserIdentifier(matchedUser.Identifier);
		}
	}
}