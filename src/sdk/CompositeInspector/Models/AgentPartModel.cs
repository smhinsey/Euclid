using Euclid.Framework.AgentMetadata;

namespace CompositeInspector.Models
{
    public class AgentPartModel : InspectorNavigationModel
    {
        public string AgentSystemName { get; set; }

        public string NextAction { get; set; }

        public IPartCollection Part { get; set; }
    }
}