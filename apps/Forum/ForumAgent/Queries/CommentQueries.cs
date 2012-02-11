using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class CommentQueries : NhQuery<Comment>
	{
		public CommentQueries(ISession session)
			: base(session)
		{
		}

		public PostDetail FindCommentsBelongingToPost(Guid forumId, Guid postId)
		{
			var result = new PostDetail { Comments = new List<Comment>() };

			var session = GetCurrentSession();

			var comments = session.QueryOver<Comment>().Where(comment => comment.PostIdentifier == postId);

			var initialPost = session.QueryOver<Post>().Where(post => post.Identifier == postId).SingleOrDefault();

			result.InitialPost = initialPost;
			result.Comments = comments.List();

			return result;
		}
	}
}