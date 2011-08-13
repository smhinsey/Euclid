using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.NHibernate;
using Euclid.Framework.Models;
using NHibernate;

namespace Euclid.Sdk.FakeAgent.Queries
{
	public class FakeReadModel : IReadModel
	{
		public virtual DateTime Created { get; set; }
		public virtual Guid Identifier { get; set; }
		public virtual DateTime Modified { get; set; }
		public virtual int RandomNumber { get; set; }
	}

	public class FakeQuery : NhQuery<FakeReadModel>
	{
		public FakeQuery(ISession session) : base(session)
		{
		}

		public IEnumerable<FakeReadModel> GetWhereRandomNumberLessThan(int number)
		{
			var session = GetCurrentSession();

			return session.QueryOver<FakeReadModel>().Where(model => model.RandomNumber < number).List();
		}
	}
}