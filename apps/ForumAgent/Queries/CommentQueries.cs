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

		public Comment FindByTitle(string title)
		{
			var session = GetCurrentSession();

			var posts = session.QueryOver<Comment>()
				.Where(post => post.Title == title);

			return posts.SingleOrDefault();
		}
	}
}