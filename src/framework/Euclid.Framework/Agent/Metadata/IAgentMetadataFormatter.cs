using System.Reflection;

namespace Euclid.Framework.Agent.Metadata
{
	public interface IAgentMetadataFormatter : IMetadataFormatter
	{
		Assembly AgentAssembly { get; }
		ICommandMetadataFormatterCollection Commands { get; }
		string DescriptiveName { get; }
		bool IsValid { get; }
		IQueryMetadataFormatterCollection Queries { get; }
		IReadModelMetadataFormatterCollection ReadModels { get; }
		string SystemName { get; }

		string GetBasicMetadata(string format);
	}
}