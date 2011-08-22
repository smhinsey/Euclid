using System;
using System.Linq;
using System.Web.Mvc;
using Euclid.Framework.AgentMetadata.Extensions;

namespace Euclid.Composites.Mvc.ActionFilters
{
	public class FormatListOfBasicAgentMetadata : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var format = filterContext.ActionParameters["format"] as string ?? string.Empty;

			if (format == string.Empty)
			{
				return;
			}

			var agentMetadata =
				AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.ContainsAgent()).Select(
					assembly => assembly.GetAgentMetadata());

			var formatter = agentMetadata.GetBasicMetadataFormatter();

			filterContext.Result = new ContentResult
				{
					Content = formatter.GetRepresentation(format), 
					ContentType = formatter.GetContentType(format), 
					ContentEncoding = formatter.GetEncoding(format)
				};

			base.OnActionExecuting(filterContext);
		}
	}
}