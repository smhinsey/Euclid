using Euclid.Framework.Agent.Metadata;

namespace MetadataComposite.Areas.Metadata.Models
{
	public class PartCollectionModel : FooterLinkModel
	{
		public string NextActionName { get; set; }
		public IPartCollection Parts { get; set; }
	}
}