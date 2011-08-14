using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composite.MvcApplication.Models
{
    public class ReadOnlyPartMetadataModel
    {
        public string AgentSystemName { get; set; }
        public ITypeMetadata TypeMetadata { get; set; }
        public string Name { get; set; }
        public string PartType { get; set; }
    }
}