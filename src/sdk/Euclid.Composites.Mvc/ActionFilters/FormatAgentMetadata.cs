using System.Linq;
using System.Web;
using System.Web.Mvc;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composites.Mvc.ActionFilters
{
    public class FormatAgentMetadata : MetadataFormatterAttributeBase
    {
        public override IMetadataFormatter GetFormatter(ActionExecutingContext filterContext)
        {
            return filterContext.ActionParameters["agentMetadata"] as IMetadataFormatter;
        }
    }
}