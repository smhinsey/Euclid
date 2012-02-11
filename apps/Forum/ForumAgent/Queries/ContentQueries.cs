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

		public IList<ForumContent>  GetActiveContent(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			return session.QueryOver<ForumContent>().Where(c => c.ForumIdentifier == forumId && c.Active).Skip(offset).Take(pageSize).List();
		}

		public IList<ForumContent> GetAllActiveContent(Guid forumId)
		{
			var session = GetCurrentSession();

			return session.QueryOver<ForumContent>().Where(c => c.ForumIdentifier == forumId && c.Active).List();
		}

		public AvailableContent List(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			return new AvailableContent
			       	{
			       		ForumIdentifier = forumId,
			       		ForumName = session.QueryOver<Forum>().Where(f => forumId == f.Identifier).SingleOrDefault().Name,
			       		ContentItems = session.QueryOver<ForumContent>().Where(c => c.ForumIdentifier == forumId).Skip(offset).Take(pageSize).List(),
			       		TotalContentItems = session.QueryOver<ForumContent>().RowCount()
			       	};
		}
	}
}