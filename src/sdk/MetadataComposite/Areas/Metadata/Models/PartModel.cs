using Euclid.Framework.Agent.Metadata;

namespace MetadataComposite.Areas.Metadata.Models
{
    public class PartModel : FooterLinkModel
    {
        public ITypeMetadata TypeMetadata { get; set; }
        public string NextActionName { get; set; }
    }
}