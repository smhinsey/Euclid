using Euclid.Framework.AgentMetadata;

namespace CompositeInspector.Models
{
    public class PartModel : InspectorNavigationModel
    {
        public string NextActionName { get; set; }

        public ITypeMetadata TypeMetadata { get; set; }
    }
}