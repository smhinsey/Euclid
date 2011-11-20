using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;
using NHibernate.Criterion;

namespace ForumAgent.Queries
{
	public class PostQueries : NhQuery<Post>
	{
		public PostQueries(ISession session)
			: base(session)
		{
		}

		public PostListing FindByAuthorIdentifier(Guid forumId, Guid authorId, int pageSize, int offset)
		{
			var result = new PostListing {Posts = new List<Post>()};

			ISession session = GetCurrentSession();

			IQueryOver<Post> posts =
				session.QueryOver<Post>().Where(p => p.AuthorIdentifier == authorId).OrderBy(p => p.Created).Desc.Skip(offset).Take(
					pageSize);

			int totalPosts = session.QueryOver<Post>().Where(p => p.AuthorIdentifier == authorId).RowCount();

			result.TotalPosts = totalPosts;

			result.Posts = posts.List();

			return result;
		}

		public Post FindByTitle(Guid forumId, string title)
		{
			ISession session = GetCurrentSession();

			IQueryOver<Post, Post> posts =
				session.QueryOver<Post>().WhereRestrictionOn(post => post.Title).IsInsensitiveLike(title, MatchMode.Exact);

			return posts.SingleOrDefault();
		}

		public IList<Post> FindPostsByCategory(Guid forumId, Guid categoryIdentifier)
		{
			ISession session = GetCurrentSession();

			IQueryOver<Post, Post> posts = session.QueryOver<Post>().Where(post => post.CategoryIdentifier == categoryIdentifier);

			return posts.List();
		}

		public int GetPostCountByAuthor(Guid forumId, Guid authorId)
		{
			ISession session = GetCurrentSession();

			int totalPosts = session.QueryOver<Post>().Where(p => p.AuthorIdentifier == authorId).RowCount();

			return totalPosts;
		}

		public PostListing GetPostListing(Guid forumId, int pageSize, int offset)
		{
			var result = new PostListing {Posts = new List<Post>()};

			ISession session = GetCurrentSession();

			IQueryOver<Post> posts =
				session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).OrderBy(p => p.Created).Desc.Skip(offset).Take(
					pageSize);

			int totalPosts = session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).RowCount();

			result.TotalPosts = totalPosts;

			result.Posts = posts.List();

			return result;
		}

		public PostListing GetPostListingByCategory(Guid forumId, string categorySlug, int pageSize, int offset)
		{
			var result = new PostListing {Posts = new List<Post>()};

			ISession session = GetCurrentSession();

			Category category =
				session.QueryOver<Category>().Where(c => c.Slug == categorySlug && c.ForumIdentifier == forumId).SingleOrDefault();

			IQueryOver<Post> posts =
				session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId && p.CategoryIdentifier == category.Identifier).
					OrderBy(p => p.Created).Desc.Skip(offset).Take(
						pageSize);

			int totalPosts = session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).RowCount();

			result.TotalPosts = totalPosts;
			result.CategoryName = category.Name;

			result.Posts = posts.List();

			return result;
		}
	}
}