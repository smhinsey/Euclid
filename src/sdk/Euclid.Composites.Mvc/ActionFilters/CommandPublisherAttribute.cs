using System;
using System.Globalization;
using System.Web.Mvc;
using AutoMapper;
using Euclid.Common.Messaging;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Cqrs;

namespace Euclid.Composites.Mvc.ActionFilters
{
	public class CommandPublisherAttribute : ActionFilterAttribute
	{
		public ICompositeApp CompositeApp { get; set; }

		public IPublisher Publisher { get; set; }

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var commandName = filterContext.HttpContext.Request.Form["partName"];

			if (filterContext.HttpContext.Request.Params == null)
			{
				throw new NullReferenceException("filterContext.HttpContext.Request.Params");
			}

			var valueProvider = new NameValueCollectionValueProvider(
				filterContext.HttpContext.Request.Params, CultureInfo.CurrentCulture);

			var inputModel = CompositeApp.GetInputModelFromCommandName(commandName, valueProvider);

			var command = CompositeApp.GetCommandForInputModel(inputModel);

			// jt:
			// if there is an HttpPostedFileBase on the inputmodel
			// with a corresponding URI property on the command
			// upload the file and store the URL

			var publicationId = Publisher.PublishMessage(command);

			filterContext.ActionParameters["publicationId"] = publicationId;
		}
	}
}