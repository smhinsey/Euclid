using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class PostQueries : NhQuery<Post>
	{
		public PostQueries(ISession session) : base(session)
		{
		}

		public IList<Post> FindPostsByCategory(string name)
		{
			var session = GetCurrentSession();

			var category = session.QueryOver<Category>()
				.Where(x => x.Name == name).SingleOrDefault();

			var posts = session.QueryOver<Post>()
				.Where(x => x.CategoryIdentifier == category.Identifier);

			return posts.List();
		}
	}
}