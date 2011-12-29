using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class UserQueries : NhQuery<ForumUser>
	{
		public UserQueries(ISession session)
			: base(session)
		{
		}

		public bool Authenticate(Guid forumId, string username, string password)
		{
			// TODO: implement safe hashing/salting and all that noise
			var session = GetCurrentSession();

			var matchedAccount =
				session.QueryOver<ForumUser>().Where(
					user =>
					user.PasswordHash == password && user.PasswordSalt == password && user.Username == username
					&& user.ForumIdentifier == forumId).SingleOrDefault();

			return matchedAccount != null;
		}

		public UserProfile FindByUserIdentifier(Guid forumId, Guid identifier)
		{
			var session = GetCurrentSession();

			var matchedUser = session.QueryOver<UserProfile>().Where(user => user.UserIdentifier == identifier);

			return matchedUser.SingleOrDefault();
		}

		public ForumUser FindByUsername(Guid forumId, string username)
		{
			var session = GetCurrentSession();

			var matchedUser = session.QueryOver<ForumUser>().Where(user => user.Username == username && user.ForumIdentifier == forumId);

			return matchedUser.SingleOrDefault();
		}

		public UserProfile UserProfileByUsername(Guid forumId, string username)
		{
			var matchedUser = FindByUsername(forumId, username);

			return FindByUserIdentifier(forumId, matchedUser.Identifier);
		}

		public IList<ForumUser> FindTopUsers(Guid forumId)
		{
			var session = GetCurrentSession();

			var users = session.QueryOver<ForumUser>().Skip(0).Take(5);

			return users.List();
		}

		public ForumUsers FindByForum(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			return new ForumUsers
			       	{
			       		ForumIdentifier = forumId,
			       		ForumName = session.QueryOver<Forum>().Where(f => f.Identifier == forumId).SingleOrDefault().Name,
			       		Users = session.QueryOver<ForumUser>().Where(user => user.ForumIdentifier == forumId).Skip(offset).Take(pageSize).List(),
			       		TotalUsers = session.QueryOver<ForumUser>().RowCount()
			       	};
		}
	}
}