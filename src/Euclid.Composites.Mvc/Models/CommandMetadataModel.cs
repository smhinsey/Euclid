using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.Models
{
    public class CommandMetadataModel
    {
        public IAgentMetadata AgentMetadata { get; set; }
        public ICommandMetadata CommandMetadata { get; set; }
    }
}