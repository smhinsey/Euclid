using System.Reflection;

namespace Euclid.Framework.Agent.Metadata
{

    public interface IAgentMetadataFormatter : IMetadataFormatter
    {
        string DescriptiveName { get; }
        string SystemName { get; }
        bool IsValid { get; }
        
        Assembly AgentAssembly { get; }
		ICommandMetadataFormatterCollection Commands { get; }
		IQueryMetadataFormatterCollection Queries { get; }
		IReadModelMetadataFormatterCollection ReadModels { get; }

        string GetBasicMetadata(string format);
    }
}