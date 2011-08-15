using System.Reflection;

namespace Euclid.Framework.Agent.Metadata
{

    public interface IAgentMetadata : IFormattedMetadataProvider
    {
        string DescriptiveName { get; }
        string SystemName { get; }
        bool IsValid { get; }
        
        Assembly AgentAssembly { get; }
		ICommandMetadataCollection Commands { get; }
		IQueryMetadataCollection Queries { get; }
		IReadModelMetadataCollection ReadModels { get; }

        string GetBasicMetadata(string format);
    }
}