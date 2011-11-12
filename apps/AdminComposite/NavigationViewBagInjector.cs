using System.Web.Mvc;
using ForumAgent.Queries;

namespace AdminComposite
{
	public class NavigationViewBagInjector : IActionFilter
	{
		public ForumQueries queries { get; set; }
		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			filterContext.Controller.ViewBag.Forums = queries.GetForums();

			filterContext.Controller.ViewBag.CurrentForumId = filterContext.GetRequestValue("forumId");

			filterContext.Controller.ViewBag.CurrentOrganizationId = filterContext.GetRequestValue("organizationId");
		}
	}

	public static class FilterContextExtensions
	{
		public static string GetRequestValue(this ActionExecutedContext filterContext, string key)
		{
			if (string.IsNullOrEmpty(key)) return string.Empty;

			var result = (string)filterContext.Controller.ControllerContext.RouteData.Values[key];

			if (string.IsNullOrEmpty(key))
			{
				result = filterContext.Controller.ControllerContext.HttpContext.Request[key];
			}

			return result;
		}
	}
} 