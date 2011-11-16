using System;
using System.Web.Mvc;
using ForumAgent.Queries;

namespace ForumComposite.Controllers
{
	public class AddUserSpecificDataAttribute : ActionFilterAttribute
	{
		public PostQueries PostQueries { get; set; }

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
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
				viewResult.ViewBag.PostCount = PostQueries.GetPostCountByAuthor(
					filterContext.Controller.ViewBag.ForumIdentifier, userId);
			}
		}
	}
}