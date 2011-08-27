using System;
using System.Globalization;
using System.Web.Mvc;
using Euclid.Common.Messaging;
using Euclid.Composites.Conversion;
using Euclid.Composites.Mvc.Extensions;

namespace Euclid.Composites.Mvc.ActionFilters
{
    public class CommandPublisherAttribute : ActionFilterAttribute
    {
        private readonly IInputModelTransformerRegistry _transformerRegistry;
        private readonly IPublisher _publisher;

        public CommandPublisherAttribute(IInputModelTransformerRegistry transformerRegistry, IPublisher publisher)
        {
            _transformerRegistry = transformerRegistry;
            _publisher = publisher;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var commandName = filterContext.HttpContext.Request.Form["partName"];

            if (filterContext.HttpContext.Request.Params == null)
            {
                throw new NullReferenceException("filterContext.HttpContext.Request.Params");
            }

            var valueProvider = new NameValueCollectionValueProvider(filterContext.HttpContext.Request.Params, CultureInfo.CurrentCulture);

            var inputModel = _transformerRegistry.GetInputModel(commandName, valueProvider);

            var command = _transformerRegistry.GetCommand(inputModel);

            var publicationId = _publisher.PublishMessage(command);

            filterContext.ActionParameters["publicationId"] = publicationId;
        }
    }
}