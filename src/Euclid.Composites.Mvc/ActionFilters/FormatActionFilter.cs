using System;
using System.Web.Mvc;
using Euclid.Composites.Mvc.Results;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.ActionFilters
{
	public class FormatInputModelAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var format = filterContext.ActionParameters["format"] as string ?? string.Empty;

			var inputModel = filterContext.ActionParameters["inputModel"] as IInputModel;

			if (inputModel == null)
			{
				throw new CannotRetrieveInputModelException();
			}

			if (!string.IsNullOrEmpty(format))
			{
				switch (format.ToLower())
				{
					case "xml":
						filterContext.Result = new XmlResult {Data = inputModel};
						break;
					case "json":
						filterContext.Result = new JsonNetResult {Data = inputModel};
						break;
					case "jsonp":
						filterContext.Result = new JsonpNetResult {Data = inputModel};
						break;
				}
			}
			base.OnActionExecuting(filterContext);
		}
	}

	public class CannotRetrieveInputModelException : Exception
	{
	}
}