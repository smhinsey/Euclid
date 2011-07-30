using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.Models
{
    public class CommandMetadataModel
    {
        public IAgentInfo AgentInfo { get; set; }
        public ICommandMetadata CommandMetadata { get; set; }
    }
}