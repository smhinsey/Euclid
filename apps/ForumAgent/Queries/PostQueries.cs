using System.Collections.Generic;
using Euclid.Common.Storage.Model;
using Euclid.Common.Storage.NHibernate;
using ForumAgent.ReadModels;

namespace ForumAgent.Queries
{
	public class PostQueries : IModelRepository<Post>
	{
		private readonly NhSimpleRepository<Post> _repository;

		public PostQueries(NhSimpleRepository<Post> repository)
		{
			_repository = repository;
		}

		public IList<Post> GetPostsByCategory(string name)
		{
			var session = _repository.GetCurrentSession();

			var category = session.QueryOver<Category>()
				.Where(x => x.Name == name).SingleOrDefault();

			var posts = session.QueryOver<Post>()
				.Where(x => x.CategoryIdentifier == category.Identifier);

			return posts.List();
		}
	}
}