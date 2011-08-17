
d:\Projects\Euclid\platform>@git.exe %*
using System.Web.Mvc;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composites.Mvc.ActionFilters
{
    public class FormatPartCollectionMetadataAttribute : MetadataFormatterAttributeBase
    {
        public override IMetadataFormatter GetFormatter(ActionExecutingContext filterContext)
        {
            var metadataCollection = filterContext.ActionParameters["partCollection"] as IPartCollection;
            if (metadataCollection == null)
            {
                throw new AgentPartMetdataNotFoundException();
            }
                
            return metadataCollection.GetFormatter();
        }
    }
}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
