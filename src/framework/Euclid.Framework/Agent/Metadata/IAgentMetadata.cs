using System.Reflection;

namespace Euclid.Framework.Agent.Metadata
{
	public interface IAgentMetadata
	{
		Assembly AgentAssembly { get; }
		ICommandMetadataCollection Commands { get; }
		string DescriptiveName { get; }
		bool IsValid { get; }
		IQueryMetadataCollection Queries { get; }
		IReadModelMetadataCollection ReadModels { get; }
		string SystemName { get; }
	}
}