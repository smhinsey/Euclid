using System.Web.Mvc;
using ForumAgent.Queries;

namespace ForumComposite.ActionFilters
{
	public class PopulateForumWideViewBag : IActionFilter
	{
		public ForumQueries ForumQueries { get; set; }
		public OrganizationQueries OrganizationQueries { get; set; }
		public OrganizationUserQueries UserQueries { get; set; }

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
		}
	}
}