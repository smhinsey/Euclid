using Euclid.Framework.AgentMetadata;

namespace CompositeInspector.Models
{
    public class PartCollectionModel : InspectorNavigationModel
    {
        public string NextActionName { get; set; }

        public IPartCollection Parts { get; set; }
    }
}