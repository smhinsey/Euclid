using Euclid.Framework.AgentMetadata;

namespace CompositeControl.Areas.CompositeControl.Models
{
	public class PartCollectionModel : FooterLinkModel
	{
		public string NextActionName { get; set; }

		public IPartCollection Parts { get; set; }
	}
}