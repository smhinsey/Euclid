using Euclid.Framework.Cqrs.NHibernate;
using NHibernate;
using StorefrontAgent.ReadModels;

namespace StorefrontAgent.Queries
{
	public class CompanyQueries : NhQuery<Company>
	{
		public CompanyQueries(ISession session)
			: base(session)
		{
		}
	}
}