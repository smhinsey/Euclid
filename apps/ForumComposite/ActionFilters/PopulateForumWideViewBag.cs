using System.Reflection;
using System.Web.Mvc;
using ForumAgent.Queries;

namespace ForumComposite.ActionFilters
{
	public class PopulateForumWideViewBag : IActionFilter
	{
		private readonly Assembly _currentAssembly;

		public PopulateForumWideViewBag()
		{
			_currentAssembly = GetType().Assembly;
		}

		public ForumQueries ForumQueries { get; set; }

		public OrganizationQueries OrganizationQueries { get; set; }

		public OrganizationUserQueries UserQueries { get; set; }

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly != _currentAssembly)
			{
				return;
			}

			var orgId = OrganizationQueries.GetIdentifierBySlug(filterContext.GetRequestValue("orgSlug"));
			var forumId = ForumQueries.GetIdentifierBySlug(filterContext.GetRequestValue("forumSlug"));
			var forum = ForumQueries.FindById(forumId);

			filterContext.Controller.ViewBag.OrganizationIdentifier = orgId;
			filterContext.Controller.ViewBag.ForumIdentifier = forumId;
			filterContext.Controller.ViewBag.ForumIsModerated = forum.Moderated;
		}
	}

	// TODO: move this elsewhere
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