using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composite.MvcApplication.Models
{
    public class PartCollectionMetadataModel
    {
        public string AgentSystemName { get; set; }
        public IAgentPartMetadataCollection Parts { get; set; }
        public string PartTypeName { get; set; }
        public string NextActionName { get; set; }
    }
}