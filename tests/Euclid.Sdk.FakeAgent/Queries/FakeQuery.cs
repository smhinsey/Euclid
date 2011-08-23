using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using Euclid.Sdk.FakeAgent.ReadModels;
using NHibernate;

namespace Euclid.Sdk.FakeAgent.Queries
{
	public class FakeQuery : NhQuery<FakeReadModel>
	{
		public FakeQuery(ISession session)
			: base(session)
		{
		}

		public IList<FakeReadModel> FindByNumber(int number)
		{
			var session = GetCurrentSession();

			return session.QueryOver<FakeReadModel>().Where(model => model.Number == number).List();
		}
	}
}