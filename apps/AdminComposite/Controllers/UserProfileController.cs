using System;
using System.Web.Mvc;
using AdminComposite.Extensions;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class UserProfileController : Controller
	{
		private readonly UserQueries _forumUserQueries;

		public UserProfileController(UserQueries forumUserQueries)
		{
			_forumUserQueries = forumUserQueries;
		}

		public ActionResult Details(Guid? forumId)
		{
			return View("_Details");
		}

		public ActionResult Invite(Guid forumId)
		{
			var userId = Request.GetLoggedInUserId();

			return View("_Invite", new RegisterForumUserInputModel
			                       	{
			                       		ForumIdentifier = forumId,
			                       		Password = "password", //TODO: better password generation required
										CreatedBy = userId
			                       	});
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			var model = _forumUserQueries.FindByForum(forumId, offset, pageSize);
			ViewBag.Pagination = new PaginationModel
			                     	{
			                     		Identifier = forumId,
			                     		ActionName = "List",
			                     		ControllerName = "UserProfile",
			                     		Offset = offset,
			                     		PageSize = pageSize,
			                     		TotalItems = model.TotalUsers
			                     	};

			return View(model);
		}
	}
}