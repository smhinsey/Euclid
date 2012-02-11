using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class ForumQueries : NhQuery<Forum>
	{
		public ForumQueries(ISession session)
			: base(session)
		{
		}

		public IList<Forum> FindByOrganization(Guid organizationId)
		{
			var session = GetCurrentSession();

			return session.QueryOver<Forum>().Where(f => f.OrganizationId == organizationId).List();
		}

		public IList<Forum> FindForums()
		{
			var session = GetCurrentSession();

			return session.QueryOver<Forum>().List();
		}

		public ForumVotingScheme GetForumVotingScheme(Guid forumIdentifier)
		{
			var session = GetCurrentSession();

			var forum = session.QueryOver<Forum>().Where(f => f.Identifier == forumIdentifier).SingleOrDefault();

			return new ForumVotingScheme
				{ ForumIdentifier = forumIdentifier, ForumName = forum.Name, CurrentScheme = getVotingScheme(forum) };
		}

		public Forum FindBySlug(Guid orgIdentifier, string forumSlug)
		{
			var session = GetCurrentSession();

			var org = session.QueryOver<Forum>()
				.Where(u => u.UrlSlug == forumSlug)
				.Where(f => f.OrganizationId == orgIdentifier)
				.SingleOrDefault();

			return org;
		}

		private static VotingScheme getVotingScheme(Forum forum)
		{
			if (forum.UpDownVoting)
			{
				return VotingScheme.UpDownVoting;
			}

			return VotingScheme.NoVoting;
		}
	}
}