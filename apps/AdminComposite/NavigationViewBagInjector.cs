using System.Reflection;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using ForumAgent.Queries;

namespace AdminComposite
{
	public class NavigationViewBagInjector : IActionFilter
	{
		public ForumQueries queries { get; set; }
		private readonly Assembly _currentAssembly;
		public NavigationViewBagInjector()
		{
			_currentAssembly = GetType().Assembly;
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly != _currentAssembly) return;

			if (filterContext.HttpContext.Request.Cookies["OrganizationUserId"] == null) return;

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