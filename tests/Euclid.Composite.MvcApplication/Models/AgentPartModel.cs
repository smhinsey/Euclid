using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composite.MvcApplication.Models
{
    public class AgentPartModel
    {
        public string AgentSystemName { get; set; }
        public string PartType { get; set; }
        public string NextAction { get; set; }
        public IAgentPartMetadataCollection PartMetadata { get; set; }
    }

    public class AgentModel
    {
        public string SystemName { get; set; }
        public string DescriptiveName { get; set; }
        public AgentPartModel Commands { get; set; }
        public AgentPartModel Queries { get; set; }
        public AgentPartModel ReadModels { get; set; }
    }
}