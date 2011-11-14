using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AdminComposite.Extensions;
using Euclid.Common.Extensions;
using ForumAgent;
using ForumAgent.Queries;

namespace AdminComposite.ActionFilters
{
	public class PopulateAdminWideViewBag : IActionFilter
	{
		public OrganizationQueries OrganizationQueries { get; set; }

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
				filterContext.Result = new RedirectToRouteResult(
												new RouteValueDictionary
													{
														{"action", "SignOut"},
														{"controller", "User"}
													});
			}
			else
			{
				var organization = OrganizationQueries.FindByIdentifier(currentUser.OrganizationIdentifier);

				if (organization == null)
				{
					throw new OrganizationNotFoundException(currentUser.OrganizationIdentifier);
				}

				filterContext.Controller.ViewBag.Forums = forumQueries.GetForums();
				filterContext.Controller.ViewBag.CurrentForumId = filterContext.GetRequestValue("forumId");
				filterContext.Controller.ViewBag.OrganizationId = organization.Identifier;
				filterContext.Controller.ViewBag.OrganizationName = organization.Name;
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

}