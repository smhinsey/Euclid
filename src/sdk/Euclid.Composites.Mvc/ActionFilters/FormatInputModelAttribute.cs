using System;
using System.Web.Mvc;
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

		    throw new NotImplementedException();

			base.OnActionExecuting(filterContext);
		}
	}
}