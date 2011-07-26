using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Metadata
{
    public class AgentMetadata : IAgentMetadata
    {
        public string FriendlyName { get; set; }
        public string SystemName { get; set; }
        public string Scheme { get; set; }
        public string CommandNamespace { get; set; }
        public string QueryNamespace { get; set; }
        public string CommandProcessorNamespace { get; set; }
    }
}
