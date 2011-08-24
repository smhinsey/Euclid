using Euclid.Framework.AgentMetadata;

namespace AgentPanel.Areas.Metadata.Models
{
	public class AgentPartModel : FooterLinkModel
	{
		public string AgentSystemName { get; set; }

		public string NextAction { get; set; }

		public IPartCollection Part { get; set; }
	}
}