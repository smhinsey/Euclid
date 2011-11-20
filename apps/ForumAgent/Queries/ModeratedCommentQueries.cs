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
			       		PageSize = pageSize,
			       		Offset = offset,
						ForumIdentifier = forumId,
			       		TotalPosts = posts.Count,
			       		Posts = posts
			       	};
		}
	}
}