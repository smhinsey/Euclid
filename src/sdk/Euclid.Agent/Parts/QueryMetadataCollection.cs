using System.Reflection;
using Euclid.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;

namespace Euclid.Agent.Parts
{
    public class QueryMetadataCollection : PartCollectionsBase<IQuery>, IQueryMetadataCollection
    {
        public QueryMetadataCollection(Assembly agent)
        {
            Initialize(agent, agent.GetQueryNamespace());
        }
    }
}