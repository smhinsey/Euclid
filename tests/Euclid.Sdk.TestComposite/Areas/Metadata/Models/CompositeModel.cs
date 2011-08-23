using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;

namespace AgentPanel.Areas.Metadata.Models
{
    public class CompositeModel
    {
        public IEnumerable<ITypeMetadata> InputModels { get; set; }

        public IEnumerable<IAgentMetadata> Agents { get; set; }

        public IEnumerable<string> ConfigurationErrors { get; set; }
    }
}