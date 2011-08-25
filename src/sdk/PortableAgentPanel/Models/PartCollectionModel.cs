using Euclid.Framework.AgentMetadata;

namespace PortableAgentPanel.Models
{
	public class PartCollectionModel : FooterLinkModel
	{
		public string NextActionName { get; set; }

		public IPartCollection Parts { get; set; }
	}
}