using System.Reflection;
using Euclid.Agent.Extensions;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;

namespace Euclid.Agent.Parts
{
	public class CommandMetadataCollection : PartCollectionsBase<ICommand>, ICommandMetadataCollection
	{
		public CommandMetadataCollection(Assembly agent)
		{
			Initialize(agent, agent.GetCommandNamespace());
		}
	}
}