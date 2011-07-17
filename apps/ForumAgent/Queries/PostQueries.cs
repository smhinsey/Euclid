using System.Collections.Generic;
using Euclid.Common.Storage.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class PostQueries : NhSimpleRepository<Post>
	{
		public PostQueries(ISession session) : base(session)
		{
		}

		public IList<Post> GetPostsByCategory(string name)
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