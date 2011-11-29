using System;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class BadgeQueries : NhQuery<ForumBadge>
	{
		public BadgeQueries(ISession session) : base(session)
		{
		}

		public AvailableBadges FindBadges(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			return new AvailableBadges
					{
						TotalBadges = session.QueryOver<ForumBadge>().Where(f => f.ForumIdentifier == forumId).RowCount(),
						Badges = session.QueryOver<ForumBadge>().Where(f => f.ForumIdentifier == forumId).Skip(offset).Take(pageSize).List(),
						ForumName = session.QueryOver<Forum>().Where(f => f.Identifier == forumId).SingleOrDefault().Name,
						ForumIdentifier = forumId
					};
		}
	}
}