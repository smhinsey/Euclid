using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class CommentQueries : NhQuery<Comment>
	{
		public CommentQueries(ISession session) : base(session)
		{
		}

		public IList<Comment> FindCommentsBelongingToPost(Guid postId)
		{
			var session = GetCurrentSession();

			var categories = session.QueryOver<Comment>()
				.Where(comment => comment.PostIdentifier == postId);

			return categories.List();
		}
	}
}