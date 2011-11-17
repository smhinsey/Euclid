using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class CategoryQueries : NhQuery<Category>
	{
		public CategoryQueries(ISession session)
			: base(session)
		{
		}

		public IList<Category> GetActiveCategories(Guid forumIdentifier)
		{
			var session = GetCurrentSession();

			return session.QueryOver<Category>().Where(c => c.Active == true && c.ForumIdentifier == forumIdentifier).List();
		}

		
	}
}