using Euclid.Framework.AgentMetadata;

namespace PortableAgentPanel.Models
{
	public class PartModel : FooterLinkModel
	{
		public string NextActionName { get; set; }

		public ITypeMetadata TypeMetadata { get; set; }
	}
}