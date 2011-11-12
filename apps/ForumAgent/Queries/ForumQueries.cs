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

		public IList<Forum> GetForums()
		{
			var session = GetCurrentSession();

			return session.QueryOver<Forum>().List();
		}

	}
}