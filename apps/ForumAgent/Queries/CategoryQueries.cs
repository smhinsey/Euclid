using System;
using System.Collections.Generic;
using Euclid.Common.Storage.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class CategoryQueries : NhSimpleRepository<Category>
	{
		public CategoryQueries(ISession session) : base(session)
		{
		}

		public IList<Category> GetTopLevelCategories()
		{
			var session = GetCurrentSession();

			var categories = session.QueryOver<Category>()
				.Where(x => x.ParentCategoryIdentifier == Guid.Empty);

			return categories.List();
		}
	}
}