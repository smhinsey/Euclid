using Euclid.Framework.AgentMetadata;

namespace AgentPanel.Areas.Metadata.Models
{
	public class PartCollectionModel : FooterLinkModel
	{
		public string NextActionName { get; set; }
		public IPartCollection Parts { get; set; }
	}
}