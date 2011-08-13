using System.Reflection;
using Euclid.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Agent.Commands
{
	public class CommandMetadataCollection : PartCollectionsBase<ICommand>, ICommandMetadataCollection
	{
		public CommandMetadataCollection(Assembly agent)
		{
			Initialize(agent, agent.GetCommandNamespace());
		}
	}

	public class QueryMetadataCollection : PartCollectionsBase<IQuery>, IQueryMetadataCollection
	{
		public QueryMetadataCollection(Assembly agent)
		{
			Initialize(agent, agent.GetQueryNamespace());
		}
	}

	public class ReadModelMetadataCollection : PartCollectionsBase<IReadModel>, IReadModelMetadataCollection
	{
		public ReadModelMetadataCollection(Assembly agent)
		{
			Initialize(agent, agent.GetReadModelNamespace());
		}
	}
}