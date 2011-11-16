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
		//public ICompositeApp CompositeApp { get; set; }

		public IInputModelMapCollection InputModelMaps { get; set; }

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

			var inputModel = InputModelMaps.GetInputModelFromCommandName(commandName, valueProvider);

			var commandMetadata = InputModelMaps.GetCommandMetadataForInputModel(inputModel.GetType());

			var command = Mapper.Map(inputModel, inputModel.GetType(), commandMetadata.Type) as ICommand;

			var publicationId = Publisher.PublishMessage(command);

			filterContext.ActionParameters["publicationId"] = publicationId;
		}
	}
}