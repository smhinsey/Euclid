using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class ForumQueries : NhQuery<Forum>
	{
		public ForumQueries(ISession session)
			: base(session)
		{
		}

		public IList<Forum> FindByOrganization(Guid organizationId)
		{
			var session = GetCurrentSession();

			return session.QueryOver<Forum>()
				.Where(f => f.OrganizationId == organizationId)
				.List();
		}

		public IList<Forum> FindForums()
		{
			var session = GetCurrentSession();

			return session.QueryOver<Forum>().List();
		}

		public Guid GetIdentifierBySlug(string slug)
		{
			var session = GetCurrentSession();

			var org = session.QueryOver<Forum>().Where(u => u.UrlSlug == slug).SingleOrDefault();

			return org.Identifier;
		}
	}
}