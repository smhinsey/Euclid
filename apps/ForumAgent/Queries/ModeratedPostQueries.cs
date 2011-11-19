using System;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class ModeratedPostQueries : NhQuery<ModeratedPost>
	{
		public ModeratedPostQueries(ISession session) : base(session)
		{
		}

		public ModeratedPosts ListUnapprovedPosts(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			return new ModeratedPosts
							{
								PageSize = pageSize,
								Offset = offset,
								TotalPosts = session.QueryOver<ModeratedPost>().Where(p => !p.Approved).RowCount(),
								Posts = session.QueryOver<ModeratedPost>()
													.Where(p => p.ForumIdentifier == forumId && !p.Approved)
													.OrderBy(p => p.Created)
													.Desc
													.Skip(offset)
													.Take(pageSize)
													.List()
							};
		}
	}
}