using System.Web.Mvc;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composites.Mvc.ActionFilters
{
	public class FormatAgentMetadataAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var format = filterContext.ActionParameters["format"] as string ?? string.Empty;

			var metadata = filterContext.ActionParameters["metadata"] as IAgentMetadata;

			if (metadata == null)
			{
				throw new CannotRetrieveMetadataException();
			}

			filterContext.Result = ResultsFormatter.FormatActionResult(metadata, format);

			base.OnActionExecuting(filterContext);
		}
	}
}