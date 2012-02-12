using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Framework.Cqrs.NHibernate;
using Euclid.Sdk.TestAgent.ReadModels;
using NHibernate;

namespace Euclid.Sdk.TestAgent.Queries
{
	public enum SingleChoice
	{
		SkipOne,
		SkipTwo,
		SkipThree
	}

	[Flags]
	public enum MultiChoice
	{
		One,
		Two,
		Three
	}

	public class TestQuery : NhQuery<TestReadModel>
	{
		public TestQuery(ISession session)
			: base(session)
		{
		}

		public IList<TestReadModel> FindByNumber(int number)
		{
			var session = GetCurrentSession();

			return session.QueryOver<TestReadModel>().Where(model => model.Number == number).List();
		}

		public IList<TestReadModel> FindByChoice(SingleChoice choice)
		{
			var session = GetCurrentSession();

			var skip = (int) choice + 1;

			return session.QueryOver<TestReadModel>().Skip(skip).List();
		}

		public IList<TestReadModel> FindByMultiChoice(MultiChoice choice)
		{
			var session = GetCurrentSession();

			var skip = Enum.GetValues(typeof (MultiChoice)).Cast<MultiChoice>().Where(value => (value & choice) == value).Sum(value => (int) value + 1);

			return session.QueryOver<TestReadModel>().Skip(skip).List();
		}
	}
}