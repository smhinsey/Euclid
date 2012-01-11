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

		public PostListing FindAllPosts(Guid forumId, int pageSize, int offset)
		{
			var result = new PostListing { Posts = new List<Post>() };

			var session = GetCurrentSession();

			var posts =
				session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).OrderBy(p => p.Created).Desc
				.Skip(offset).Take(pageSize);

			var totalPosts = session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).RowCount();

			result.TotalPosts = totalPosts;

			result.Posts = posts.List();

			return result;
		}

		public PostDetail FindByIdentifier(Guid forumId, Guid postId)
		{
			var session = GetCurrentSession();

			var post =
				session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).Where(p => p.Identifier == postId).
					SingleOrDefault();

			var comments =
				session.QueryOver<Comment>().Where(c => c.ForumIdentifier == forumId).Where(c => c.PostIdentifier == postId).List();

			var category = session.QueryOver<Category>().Where(c => c.Identifier == post.CategoryIdentifier).SingleOrDefault();

			return new PostDetail { InitialPost = post, Comments = comments, Created = post.Created, Category = category };
		}

		public Post FindByTitle(Guid forumId, string title)
		{
			var session = GetCurrentSession();

			var posts = session.QueryOver<Post>().WhereRestrictionOn(post => post.Title).IsInsensitiveLike(
				title, MatchMode.Exact);

			return posts.SingleOrDefault();
		}

		public PostListing FindControversialPosts(Guid forumId, int pageSize, int offset)
		{
			var result = new PostListing { Posts = new List<Post>() };

			var session = GetCurrentSession();

			var posts =
				session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId)
				.OrderBy(p => p.TotalVotes).Desc
				.Skip(offset).Take(pageSize);

			var totalPosts = session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).RowCount();

			result.TotalPosts = totalPosts;

			result.Posts = posts.List();

			return result;
		}

		public PostListing FindPopularPosts(Guid forumId, int pageSize, int offset)
		{
			var result = new PostListing { Posts = new List<Post>() };

			var session = GetCurrentSession();

			var posts =
				session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId)
				.OrderBy(p => p.Score).Desc
				.Skip(offset).Take(pageSize);

			var totalPosts = session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).RowCount();

			result.TotalPosts = totalPosts;

			result.Posts = posts.List();

			return result;
		}

		public PostListing FindPostsInCategory(Guid forumId, string categorySlug, int pageSize, int offset)
		{
			var result = new PostListing { Posts = new List<Post>() };

			var session = GetCurrentSession();

			var category =
				session.QueryOver<Category>().Where(c => c.Slug == categorySlug && c.ForumIdentifier == forumId).SingleOrDefault();

			var posts =
				session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId && p.CategoryIdentifier == category.Identifier).
					OrderBy(p => p.Created).Desc.Skip(offset).Take(pageSize);

			var totalPosts = session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).RowCount();

			result.TotalPosts = totalPosts;
			result.CategoryName = category.Name;

			result.Posts = posts.List();

			return result;
		}

		public PostListing FindPostsInTag(Guid forumId, string tag, int pageSize, int offset)
		{
			var result = new PostListing { Posts = new List<Post>() };

			var session = GetCurrentSession();

			var posts =
				session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).Where(Restrictions.On<Post>(p => p.Tags).IsInsensitiveLike(tag)).
					OrderBy(p => p.Created).Desc.Skip(offset).Take(pageSize);

			var totalPosts = session.QueryOver<Post>().Where(p => p.ForumIdentifier == forumId).RowCount();

			result.TotalPosts = totalPosts;
			result.TagName = tag;

			result.Posts = posts.List();

			return result;
		}
	}
}