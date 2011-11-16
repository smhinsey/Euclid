using System.Web.Mvc;

namespace AdminComposite.Extensions
{
	public static class FilterContextExtensions
	{
		public static string GetRequestValue(this ActionExecutingContext filterContext, string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return string.Empty;
			}

			var result = (string)filterContext.Controller.ControllerContext.RouteData.Values[key];

			if (string.IsNullOrEmpty(key))
			{
				result = filterContext.Controller.ControllerContext.HttpContext.Request[key];
			}

			return result;
		}
	}
}