using Euclid.Framework.AgentMetadata;

namespace JsonCompositeInspector.Models
{
	public class AgentPartModel
	{
		public string AgentSystemName { get; set; }

		public string NextAction { get; set; }

		public IPartCollection Part { get; set; }
	}
}