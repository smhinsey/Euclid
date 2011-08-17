using Euclid.Framework.Agent.Metadata;

namespace MetadataComposite.Areas.Metadata.Models
{
    public class PartCollectionModel : FooterLinkModel
	{
		public IPartCollection Parts { get; set; }
        public string NextActionName { get; set; }
	}
}