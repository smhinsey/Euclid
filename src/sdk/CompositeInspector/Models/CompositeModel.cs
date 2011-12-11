using System.Collections.Generic;
using Euclid.Composites;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;

namespace CompositeInspector.Models
{
	public class CompositeModel
	{
		public IEnumerable<IAgentMetadata> Agents { get; set; }

		public List<CommandAndInputModel> CommandsAndInputModels { get; set; }

		public IEnumerable<string> ConfigurationErrors { get; set; }

		public string Description { get; set; }

		public string Name { get; set; }

		public CompositeAppSettings Settings { get; set; }
	}

	public class CommandAndInputModel
	{
		public IPartMetadata Command { get; set; }

		public ITypeMetadata InputModel { get; set; }
	}
}