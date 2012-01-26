using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;

namespace CompositeInspector.Models
{
	public class CompositeHome
	{
		public IEnumerable<IAgentMetadata> Agents { get; set; }

		public string CompositeDescription { get; set; }

		public string CompositeName { get; set; }

		public IEnumerable<string> ConfigurationErrors { get; set; }

		public bool IsValid { get; set; }
	}
}