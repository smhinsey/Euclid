using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composite.MvcApplication.Models
{
    public class PartModel : FooterLinkModel
    {
        public ITypeMetadata TypeMetadata { get; set; }
        public string NextActionName { get; set; }
    }
}