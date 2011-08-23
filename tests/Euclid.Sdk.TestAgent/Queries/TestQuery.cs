using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using Euclid.Sdk.TestAgent.ReadModels;
using NHibernate;

namespace Euclid.Sdk.TestAgent.Queries
{
	public class TestQuery : NhQuery<TestReadModel>
	{
		public TestQuery(ISession session) : base(session)
		{
		}

		public IList<TestReadModel> FindByNumber(int number)
		{
			var session = GetCurrentSession();

			return session.QueryOver<TestReadModel>().Where(model => model.Number == number).List();
		}
	}
}