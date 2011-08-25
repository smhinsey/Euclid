using System.Collections.Generic;
using Euclid.Composites;
using Euclid.Framework.AgentMetadata;

namespace CompositeControl.Areas.CompositeControl.Models
{
	public class CompositeModel
	{
		public IEnumerable<IAgentMetadata> Agents { get; set; }

		public IEnumerable<string> ConfigurationErrors { get; set; }

		public IEnumerable<ITypeMetadata> InputModels { get; set; }

		public CompositeAppSettings Settings { get; set; }
	}
}