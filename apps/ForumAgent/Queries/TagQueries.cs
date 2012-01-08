using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;
using NHibernate.Criterion;

namespace ForumAgent.Queries
{
	public class TagQueries : NhQuery<Tag>
	{
		public TagQueries(ISession session)
			: base(session)
		{
		}

		public AvailableTags FindActiveTags(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			var forumName = session.QueryOver<Forum>().Where(f => f.Identifier == forumId).SingleOrDefault().Name;

			var tags =
				session.QueryOver<Tag>().Where(tag => tag.ForumIdentifier == forumId).Where(tag => tag.Active).Skip(offset).Take(
					pageSize).List();

			var totalTags =
				session.QueryOver<Tag>().Where(tag => tag.ForumIdentifier == forumId).Where(tag => tag.Active).RowCount();

			return new AvailableTags { ForumIdentifier = forumId, ForumName = forumName, Tags = tags, TotalTags = totalTags };
		}

		// TODO: we need to be using futures here to avoid the n+1, although it's not too bad
		public Tag FindByName(Guid forumId, string name)
		{
			var session = GetCurrentSession();

			var tag =
				session.QueryOver<Tag>().Where(t => t.ForumIdentifier == forumId).Where(t => t.Name == name).SingleOrDefault();

			return tag;
		}

		public IList<TagDetail> FindTagsForForum(Guid forumId, int postsPerCategory)
		{
			var session = GetCurrentSession();

			var tags = session.QueryOver<Tag>().Where(c => c.Active && c.ForumIdentifier == forumId).List();

			return (from tag in tags
			        let posts =
			        	session.QueryOver<Post>().Where(Restrictions.On<Post>(p => p.Tags).IsInsensitiveLike(tag.Name)).OrderBy(p => p.Created).Desc.Take(
			        		postsPerCategory).List()
			        select new TagDetail { Tag = tag, Posts = posts }).ToList();
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