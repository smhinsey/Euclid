using System.Web.Mvc;

namespace Euclid.Composites.Mvc.ActionFilters
{
	public class FormatExceptionAttribute : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
			{
				filterContext.Result = new JsonResult()
										{
											ContentType = "application/json",
											Data = new
													{
														name = filterContext.Exception.GetType().Name,
														message = filterContext.Exception.Message,
														callstack = filterContext.Exception.StackTrace,
														exception = true
													},
											JsonRequestBehavior = JsonRequestBehavior.AllowGet
										};

				filterContext.ExceptionHandled = true;
			}
			else
			{
				base.OnException(filterContext);
			}
		}
	}
}