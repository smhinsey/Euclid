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

		public Category FindBySlug(Guid forumId, string slug)
		{
			var session = GetCurrentSession();

			return
				session.QueryOver<Category>().Where(c => c.ForumIdentifier == forumId).Where(c => c.Slug == slug).SingleOrDefault();
		}

		public IList<Category> GetActiveCategories(Guid forumIdentifier, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			return
				session.QueryOver<Category>().Where(c => c.Active && c.ForumIdentifier == forumIdentifier).Skip(offset).Take(
					pageSize).List();
		}

		public AvailableCategories List(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			return new AvailableCategories
				{
					ForumIdentifier = forumId,
					ForumName = session.QueryOver<Forum>().Where(f => f.Identifier == forumId).SingleOrDefault().Name,
					Categories =
						session.QueryOver<Category>().Where(c => c.ForumIdentifier == forumId).Skip(offset).Take(pageSize).List(),
					TotalCategories = session.QueryOver<Category>().Where(c => c.ForumIdentifier == forumId).RowCount()
				};
		}
	}
}