using System.Collections.Generic;
using Euclid.Composites;
using Euclid.Framework.AgentMetadata;

namespace CompositeInspector.Models
{
    public class CompositeModel : InspectorNavigationModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<IAgentMetadata> Agents { get; set; }

        public IEnumerable<string> ConfigurationErrors { get; set; }

        public IEnumerable<ITypeMetadata> InputModels { get; set; }

        public CompositeAppSettings Settings { get; set; }
    }
}