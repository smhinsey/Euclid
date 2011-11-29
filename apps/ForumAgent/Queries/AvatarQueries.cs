﻿using System;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class AvatarQueries : NhQuery<ForumAvatar>
	{
		public AvatarQueries(ISession session) : base(session)
		{
		}

		public AvailableAvatars FindAvatarsForForum(Guid forumId, int offset, int pageSize)
		{
			var session = GetCurrentSession();

			return new AvailableAvatars
			       	{
			       		Avatars = session.QueryOver<ForumAvatar>().Where(a => a.ForumIdentifier == forumId).List(),
			       		TotalAvatars = session.QueryOver<ForumAvatar>().Where(a => a.ForumIdentifier == forumId).RowCount(),
			       		ForumName = session.QueryOver<Forum>().Where(f => f.Identifier == forumId).SingleOrDefault().Name,
			       	};
		}
	}
}