
d:\Projects\Euclid\platform>@git.exe %*
using System.Web.Mvc;
using Euclid.Composites.Mvc.Binders;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composites.Mvc.ActionFilters
{
    public class FormatPartMetadataAttribute : MetadataFormatterAttributeBase
    {
        public override IMetadataFormatter GetFormatter(ActionExecutingContext filterContext)
        {
            var partMetadata = filterContext.ActionParameters["typeMetadata"] as ITypeMetadata;

            if (partMetadata == null)
            {
                throw new AgentPartMetdataNotFoundException();
            }

            return partMetadata.GetFormatter();
        }
    }
}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
