namespace Euclid.Framework.Metadata
{
	public interface IAgentMetadata
	{
		ICommandMetadataCollection Commands { get; }
		string DescriptiveName { get; }
		bool IsValid { get; }

		IAgentPartMetadataCollection Queries { get; }
		string SystemName { get; }
	}
}