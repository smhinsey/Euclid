using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Euclid.Common.Extensions;
using ForumAgent.Queries;

namespace AdminComposite.ActionFilters
{
	public class PopulateAdminWideViewBag : IActionFilter
	{
		private readonly Assembly _currentAssembly;

		public PopulateAdminWideViewBag()
		{
			_currentAssembly = GetType().Assembly;
		}

		public ForumQueries ForumQueries { get; set; }

		public OrganizationUserQueries UserQueries { get; set; }

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly != _currentAssembly)
			{
				return;
			}

			if (!HttpContext.Current.User.Identity.IsAuthenticated)
			{
				return;
			}

			var currentUser = UserQueries.FindByUsername(HttpContext.Current.User.Identity.Name);

			if (currentUser == null)
			{
				filterContext.Result =
					new RedirectToRouteResult(new RouteValueDictionary { { "action", "SignOut" }, { "controller", "User" } });
			}
			else
			{
				filterContext.Controller.ViewBag.Forums = ForumQueries.GetForums();
				filterContext.Controller.ViewBag.CurrentForumId = filterContext.GetRequestValue("forumId");
				filterContext.Controller.ViewBag.OrganizationId = currentUser.OrganizationIdentifier;
				filterContext.Controller.ViewBag.UserId = currentUser.Identifier;
				filterContext.Controller.ViewBag.FirstName = currentUser.FirstName;
				filterContext.Controller.ViewBag.LastName = currentUser.LastName;
				filterContext.Controller.ViewBag.Gravatar = string.Format(
					"http://www.gravatar.com/avatar/{0}?s=45", currentUser.Email.GetMd5());
			}
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}
	}

	public static class FilterContextExtensions
	{
		public static string GetRequestValue(this ActionExecutedContext filterContext, string key)
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