using System.Reflection;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Framework.Metadata
{
    public interface IAgentMetadata
    {
        string DescriptiveName { get; }
        string SystemName { get; }
        bool IsValid { get; }

        ICommandMetadataCollection Commands { get; }
        IAgentPartMetadataCollection Queries { get; }
        IAgentPartMetadataCollection CommandProcessors { get; }
    }
}