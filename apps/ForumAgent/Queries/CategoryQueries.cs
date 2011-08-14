using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class CategoryQueries : NhQuery<Category>
	{
		public CategoryQueries(ISession session) : base(session)
		{
		}

		public IList<Category> FindTopLevelCategories()
		{
			var session = GetCurrentSession();

			var categories = session.QueryOver<Category>()
				.Where(category => category.ParentCategoryIdentifier == Guid.Empty);

			return categories.List();
		}
	}
}