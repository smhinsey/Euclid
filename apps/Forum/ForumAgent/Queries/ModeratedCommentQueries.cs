using System;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class ModeratedCommentQueries : NhQuery<ModeratedComment>
	{
		public ModeratedCommentQueries(ISession session) : base(session)
		{
		}

		public ModeratedItems ListUnapprovedComments(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			var posts = session.QueryOver<ModeratedComment>()
									.Where(p => p.ForumIdentifier == forumId && !p.Approved)
									.OrderBy(p => p.Created)
									.Desc
									.Skip(offset)
									.Take(pageSize)
									.List<dynamic>();

			return new ModeratedItems
			       	{
						ForumIdentifier = forumId,
						ForumName = session.QueryOver<Forum>().Where(f => f.Identifier == forumId).SingleOrDefault().Name,
						PageSize = pageSize,
			       		Offset = offset,
						TotalPosts = session.QueryOver<ModeratedPost>().Where(p => p.ForumIdentifier == forumId && !p.Approved).RowCount(),
			       		Posts = posts
			       	};
		}
	}
}