using System.Web.Mvc;
using Euclid.Framework.AgentMetadata;

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