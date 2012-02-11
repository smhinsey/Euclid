using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Framework.Cqrs.NHibernate;
using ForumAgent.ReadModels;
using NHibernate;

namespace ForumAgent.Queries
{
	public class ThemeQueries : NhQuery<ForumTheme>
	{
		private const string ThemePathFormat = "/Content/chromatron/img/forum-themes/{0}.png";

		public ThemeQueries(ISession session) : base(session)
		{
		}

		public IList<ForumTheme> GetForumThemes(Guid forumId)
		{
			var session = GetCurrentSession();

			var forum = session.QueryOver<Forum>().Where(f => f.Identifier == forumId).SingleOrDefault();

			var themes = new List<ForumTheme>
			             	{
			             		getForumTheme("Default", "1912959A-9242-43D5-9D37-487526604446", forum),

											getForumTheme("Swiss-Blue", "1912959A-9242-43D5-9D37-487526604446", forum),

											getForumTheme("Swiss-Green", "1912959A-9242-43D5-9D37-487526604446", forum),

											getForumTheme("Swiss-Purple", "1912959A-9242-43D5-9D37-487526604446", forum),

											getForumTheme("No-Theme", "1912959A-9242-43D5-9D37-487526604446", forum),
			             	};

			if (!themes.Any(t=>t.IsCurrent))
			{
				themes[0].IsCurrent = true;
			}

			return themes;
		}

		private static ForumTheme getForumTheme(string name, string id, Forum forum)
		{
			return new ForumTheme
			       	{
			       		Identifier = Guid.Parse(id),
			       		Name = name,
			       		IsCurrent = forum.Theme.Equals(name, StringComparison.InvariantCultureIgnoreCase),
			       		PreviewUrl = string.Format(ThemePathFormat, name)
			       	};
		}
	}
}
