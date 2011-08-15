using System;
using System.Linq;
using System.Web.Mvc;
using Euclid.Agent.Extensions;

namespace Euclid.Composites.Mvc.ActionFilters
{
	public class FormatListAgentMetadataAttribute : ActionFilterAttribute
	{
	    public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var format = filterContext.ActionParameters["format"] as string ?? string.Empty;

	        var metadata = AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .Where(assembly => assembly.ContainsAgent()).Select(assembly => assembly.GetAgentMetadata());

            filterContext.Result = ResultsFormatter.FormatActionResult(metadata, format);

			base.OnActionExecuting(filterContext);
		}
	}
}