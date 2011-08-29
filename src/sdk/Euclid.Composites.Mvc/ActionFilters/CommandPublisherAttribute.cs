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
		public IPublisher Publisher { get; set; }

		public IInputModelTransformerRegistry TransformerRegistry { get; set; }

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var commandName = filterContext.HttpContext.Request.Form["partName"];

			if (filterContext.HttpContext.Request.Params == null)
			{
				throw new NullReferenceException("filterContext.HttpContext.Request.Params");
			}

			var valueProvider = new NameValueCollectionValueProvider(
				filterContext.HttpContext.Request.Params, CultureInfo.CurrentCulture);

			var inputModel = TransformerRegistry.GetInputModel(commandName, valueProvider);

			var command = TransformerRegistry.GetCommand(inputModel);

			var publicationId = Publisher.PublishMessage(command);

			filterContext.ActionParameters["publicationId"] = publicationId;
		}
	}
}