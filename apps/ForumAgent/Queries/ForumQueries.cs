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
	}
}