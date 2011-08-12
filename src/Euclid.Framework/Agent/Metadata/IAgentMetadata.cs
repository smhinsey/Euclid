using System.Reflection;

namespace Euclid.Framework.Agent.Metadata
{
	public interface IAgentMetadata
	{
		Assembly AgentAssembly { get; }
		ICommandMetadataCollection Commands { get; }
		string DescriptiveName { get; }
		bool IsValid { get; }

		IAgentPartMetadataCollection Queries { get; }
		string SystemName { get; }
	}
}