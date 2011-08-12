using System.Reflection;

namespace Euclid.Framework.Agent.Metadata
{
	public interface IAgentMetadata
	{
		Assembly AgentAssembly { get; }
		string DescriptiveName { get; }
		bool IsValid { get; }
		string SystemName { get; }

		ICommandMetadataCollection Commands { get; }
		IQueryMetadataCollection Queries { get; }
		IReadModelMetadataCollection ReadModels { get; }
	}
}