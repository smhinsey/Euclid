using System.Web.Mvc;
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