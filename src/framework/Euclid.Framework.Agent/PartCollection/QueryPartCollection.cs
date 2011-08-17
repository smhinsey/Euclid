using System.Reflection;
using Euclid.Framework.Cqrs;

namespace Euclid.Framework.Agent.PartCollection
{
	public class QueryPartCollection : PartCollectionBase<IQuery>
	{
		public QueryPartCollection(Assembly agent, string queryNamespace)
			: base(agent, queryNamespace)
		{
		}

		public override string DescriptiveName
		{
			get { return "Queries"; }
		}
	}
}