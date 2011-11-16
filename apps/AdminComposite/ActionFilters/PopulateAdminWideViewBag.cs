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
		private readonly Assembly _currentAssembly;
		public ForumQueries ForumQueries { get; set; }
		public OrganizationUserQueries UserQueries { get; set; }
		public OrganizationQueries OrganizationQueries { get; set; }
		public PopulateAdminWideViewBag()
		{
			_currentAssembly = GetType().Assembly;
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly != _currentAssembly)
			{
				return;
			}

			if (!HttpContext.Current.User.Identity.IsAuthenticated)
			{
				return;
			}

			if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Authentication")
			{
				return;
			}

			var currentUser = UserQueries.FindByUsername(HttpContext.Current.User.Identity.Name);

			if (currentUser == null)
			{
				filterContext.Result = new RedirectToRouteResult(
												new RouteValueDictionary
													{
														{"action", "DoSignout"},
														{"controller", "Authentication"}
													});
			}
			else
			{
				var organization = OrganizationQueries.FindById(currentUser.OrganizationIdentifier);

				if (organization == null)
				{
					throw new OrganizationNotFoundException(currentUser.OrganizationIdentifier);
				}

				filterContext.Controller.ViewBag.Forums = ForumQueries.GetForums();
				filterContext.Controller.ViewBag.CurrentForumId = filterContext.GetRequestValue("forumId");
				filterContext.Controller.ViewBag.OrganizationId = organization.Identifier;
				filterContext.Controller.ViewBag.OrganizationSlug = organization.Slug;
				filterContext.Controller.ViewBag.UserId = currentUser.Identifier;
				filterContext.Controller.ViewBag.FirstName = currentUser.FirstName;
				filterContext.Controller.ViewBag.LastName = currentUser.LastName;
				filterContext.Controller.ViewBag.Gravatar = string.Format("http://www.gravatar.com/avatar/{0}?s=45", currentUser.Email.GetMd5());
			}
		}
	}

}