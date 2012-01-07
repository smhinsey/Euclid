using System;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class TagQueries : NhQuery<Tag>
	{
		public TagQueries(ISession session)
			: base(session)
		{
		}

		public AvailableTags List(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			var forumName = session.QueryOver<Forum>().Where(f => f.Identifier == forumId).SingleOrDefault().Name;

			var tags = session.QueryOver<Tag>().Where(c => c.ForumIdentifier == forumId).Skip(offset).Take(pageSize).List();

			var totalTags = session.QueryOver<Tag>().Where(c => c.ForumIdentifier == forumId).RowCount();

			return new AvailableTags { ForumIdentifier = forumId, ForumName = forumName, Tags = tags, TotalTags = totalTags };
		}
	}
}