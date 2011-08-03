namespace Euclid.Framework.Metadata
{
    public interface IAgentMetadata
    {
        string DescriptiveName { get; }
        string SystemName { get; }
        bool IsValid { get; }

        ICommandMetadataCollection Commands { get; }
        IAgentPartMetadataCollection Queries { get; }
    }
}