using Euclid.Framework.Agent.Metadata;

namespace MetadataComposite.Areas.Metadata.Models
{
	public class PartModel : FooterLinkModel
	{
		public string NextActionName { get; set; }
		public ITypeMetadata TypeMetadata { get; set; }
	}
}