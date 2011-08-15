using System.Web.Mvc;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composites.Mvc.ActionFilters
{
    public class FormatPartCollectionMetadataAttribute : MetadataFormatterAttributeBase
    {
        public override IFormattedMetadataProvider GetFormatter(ActionExecutingContext filterContext)
        {
            return filterContext.ActionParameters["partCollection"] as IFormattedMetadataProvider;
        }
    }
}