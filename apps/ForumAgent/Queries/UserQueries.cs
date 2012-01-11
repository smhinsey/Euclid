using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.Domain.Entities;
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

		public IList<ForumUserFavorite> FindUserFavorites(Guid forumId, Guid userId)
		{
			var session = GetCurrentSession();

			return
				session.QueryOver<ForumUserFavorite>().Where(f => f.ForumIdentifier == forumId).Where(
					f => f.UserIdentifier == userId).OrderBy(f => f.Created).Desc.List();
		}

		public IList<ForumUserFriend> FindUserFriends(Guid forumId, Guid userId)
		{
			var session = GetCurrentSession();

			return
				session.QueryOver<ForumUserFriend>().Where(f => f.ForumIdentifier == forumId).Where(
					f => f.UserIdentifier == userId).OrderBy(f => f.Created).Desc.List();
		}

		public IList<ForumUserFavorite> FindFavoritesRelatedToPost(Guid forumId, Guid userId, Guid postId)
		{
			var session = GetCurrentSession();

			return
				session.QueryOver<ForumUserFavorite>().Where(f => f.ForumIdentifier == forumId).Where(
					f => f.UserIdentifier == userId).Where(f => f.AssociatedPostIdentifier == postId).List();
		}

		public bool IsFavoritePost(Guid forumId, Guid userId, Guid postId)
		{
			var favorites = FindFavoritesRelatedToPost(forumId, userId, postId);

			return favorites.Count > 0;
		}

		public UserProfile FindProfileByUserIdentifier(Guid forumId, Guid identifier)
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

			return FindProfileByUserIdentifier(forumId, matchedUser.Identifier);
		}

		public IList<ForumUser> FindTopUsers(Guid forumId)
		{
			var session = GetCurrentSession();

			var users = session.QueryOver<ForumUser>().Where(u => u.ForumIdentifier == forumId).Skip(0).Take(5);

			return users.List();
		}

		public ForumUserListing FindForumUsers(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			var users = session.QueryOver<ForumUser>().Where(u => u.ForumIdentifier == forumId).Skip(offset).Take(pageSize).List();
			var totalUsers = session.QueryOver<ForumUser>().Where(u => u.ForumIdentifier == forumId).RowCount();

			return new ForumUserListing() { Users = users, TotalUsers = totalUsers };
		}

		public IList<ForumUserAction> FindUserActivity(Guid forumId, Guid userIdentifier)
		{
			var session = GetCurrentSession();

			var activity = session.QueryOver<ForumUserAction>()
				.Where(a => a.ForumIdentifier == forumId)
				.Where(a => a.UserIdentifier == userIdentifier)
				.OrderBy(a => a.ActivityOccurredOn).Desc;

			return activity.List();
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