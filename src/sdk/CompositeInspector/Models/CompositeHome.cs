using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;

namespace JsonCompositeInspector.Models
{
	public class CompositeHome
	{
		public string CompositeName { get; set; }
		public string CompositeDescription { get; set; }
		public bool IsValid { get; set; }
		public IEnumerable<string> ConfigurationErrors { get; set; }
		public IEnumerable<IAgentMetadata> Agents { get; set; }
	}
}