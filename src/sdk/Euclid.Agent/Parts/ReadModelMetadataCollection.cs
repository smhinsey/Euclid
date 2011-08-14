using System.Reflection;
using Euclid.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Models;

namespace Euclid.Agent.Parts
{
	public class ReadModelMetadataCollection : PartCollectionsBase<IReadModel>, IReadModelMetadataCollection
	{
		public ReadModelMetadataCollection(Assembly agent)
		{
			Initialize(agent, agent.GetReadModelNamespace());
		}
	}
}