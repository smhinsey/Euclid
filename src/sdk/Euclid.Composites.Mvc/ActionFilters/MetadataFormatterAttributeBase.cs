using System.Web.Mvc;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composites.Mvc.ActionFilters
{
    public abstract class MetadataFormatterAttributeBase : ActionFilterAttribute
    {
        public abstract IMetadataFormatter GetFormatter(ActionExecutingContext filterContext);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var format = filterContext.ActionParameters["format"] as string ?? string.Empty;

            if (format == string.Empty) return;

            var formatter = GetFormatter(filterContext);

            if (formatter == null)
            {
                throw new AgentMetadataNotFoundException();
            }

            filterContext.Result = new ContentResult()
                                       {
                                           Content = formatter.GetRepresentation(format),
                                           ContentType = formatter.GetContentType(format),
                                           ContentEncoding = formatter.GetEncoding(format)
                                       };

            base.OnActionExecuting(filterContext);
        }
    }
}