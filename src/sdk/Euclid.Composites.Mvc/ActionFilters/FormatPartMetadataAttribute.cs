using System.Web.Mvc;
using Euclid.Composites.Mvc.Binders;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composites.Mvc.ActionFilters
{
    public class FormatPartMetadataAttribute : MetadataFormatterAttributeBase
    {
        public override IFormattedMetadataProvider GetFormatter(ActionExecutingContext filterContext)
        {
            var partMetadata = filterContext.ActionParameters["typeMetadata"] as ITypeMetadata;

            if (partMetadata == null)
            {
                throw new AgentPartMetdataNotFoundException();
            }

            var partType = filterContext.ActionParameters["partType"] as string;

            if (string.IsNullOrEmpty(partType))
            {
                throw new AgentPartTypeNotSpecifiedException();
            }

            switch(partType.ToLower())
            {
                case "commands":
                    partType = "command";
                    break;
                case "queries":
                    partType = "query";
                    break;
                case "readmodels":
                    partType = "readmodel";
                    break;
            }

            return partMetadata.GetMetadataFormatter(partType);
        }
    }
}