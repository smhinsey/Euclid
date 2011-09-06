using System;
using System.Web.Mvc;
using ForumAgent.Queries;

namespace ForumComposite.Controllers
{
	public class AddUserSpecificDataAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			// SELF there's a better way to do this, but i'm lazy

			var postQueries = DependencyResolver.Current.GetService<PostQueries>();

			var viewResult = filterContext.Result as ViewResult;

			var context = filterContext.RequestContext.HttpContext;

			var userIdCookie = context.Request.Cookies["ForumUserId"];

			if (userIdCookie == null)
			{
				return;
			}

			var userId = Guid.Parse(userIdCookie.Value);

			if (viewResult != null)
			{
				viewResult.ViewBag.UserIdentifier = userId;
				viewResult.ViewBag.PostCount = postQueries.GetPostCountByAuthor(userId);
			}
		}
	}
}