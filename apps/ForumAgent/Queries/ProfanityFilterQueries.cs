using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class ProfanityFilterQueries : NhQuery<StopWord>
	{
		public ProfanityFilterQueries(ISession session)
			: base(session)
		{
		}

		public AvailableStopWords List(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			var forumName = session.QueryOver<Forum>().Where(f => f.Identifier == forumId).SingleOrDefault().Name;

			var stopWords = session.QueryOver<StopWord>().Where(c => c.ForumIdentifier == forumId).Skip(offset).Take(pageSize).List();

			var totalStopWords = session.QueryOver<StopWord>().Where(c => c.ForumIdentifier == forumId).RowCount();

			return new AvailableStopWords { ForumIdentifier = forumId, ForumName = forumName, StopWords = stopWords, TotalStopWords = totalStopWords };
		}

		public IList<StopWord> FindAllActiveInForum(Guid forumId)
		{
			var session = GetCurrentSession();
			return session.QueryOver<StopWord>().Where(stopWord => stopWord.ForumIdentifier == forumId).Where(stopWord => stopWord.Active).List();
		}
	}
}