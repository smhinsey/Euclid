using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class ContentQueries : NhQuery<ForumContent>
	{
		public ContentQueries(ISession session) : base(session)
		{
		}

		public IList<ForumContent> GetByLocation(Guid forumId, string location)
		{
			var session = GetCurrentSession();

			return session.QueryOver<ForumContent>().Where(c => c.ContentLocation == location && c.ForumIdentifier == forumId).List();
		}

		public IList<ForumContent>  GetActiveContent(Guid forumId)
		{
			var session = GetCurrentSession();

			return session.QueryOver<ForumContent>().Where(c => c.ForumIdentifier == forumId && c.Active).List();
		}
	}
}