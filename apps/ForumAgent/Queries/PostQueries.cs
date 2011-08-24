using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class PostQueries : NhQuery<Post>
	{
		public PostQueries(ISession session)
			: base(session)
		{
		}

		public Post FindByTitle(string title)
		{
			var session = GetCurrentSession();

			var posts = session.QueryOver<Post>().Where(post => post.Title == title);

			return posts.SingleOrDefault();
		}

		public IList<Post> FindPostsByCategory(Guid categoryIdentifier)
		{
			var session = GetCurrentSession();

			var posts = session.QueryOver<Post>().Where(post => post.CategoryIdentifier == categoryIdentifier);

			return posts.List();
		}
	}
}
