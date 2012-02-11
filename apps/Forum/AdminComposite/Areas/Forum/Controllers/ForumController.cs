using System;
using System.Web.Mvc;
using AdminComposite.Areas.Forum.InputModels;
using AdminComposite.Controllers;
using AdminComposite.Extensions;
using ForumAgent;
using ForumAgent.Queries;

namespace AdminComposite.Areas.Forum.Controllers
{
	[Authorize]
	public class ForumController : AdminController
	{
		private readonly ForumQueries _forumQueries;
		private readonly ForumHostQueries _forumHostQueries;

		public ForumController(ForumQueries forumQueries, ForumHostQueries forumHostQueries)
		{
			_forumQueries = forumQueries;
			_forumHostQueries = forumHostQueries;
		}

		public ActionResult AuthenticationProviders(Guid forumId)
		{
			ViewBag.ForumName = _forumQueries.FindById(forumId).Name;
			return View();
		}

		public ActionResult Create()
		{
			var userId = Request.GetLoggedInUserId();

			return
				View(
					new CreateForumInputModel
						{
							AvailableHosts = _forumHostQueries.GetHosts(),
							UrlHostName = string.Empty,
							OrganizationId = AdminInfo.OrganizationId,
							Description = " ",
							CreatedBy = userId,
							VotingScheme = VotingScheme.UpDownVoting,
							Theme = "Default"
						});
		}

		public ActionResult Details(Guid forumId)
		{
			var forum = _forumQueries.FindById(forumId);
			var model = new UpdateForumInputModel
				{
					Description = forum.Description,
					Name = forum.Name,
					UrlSlug = forum.UrlSlug,
					UrlHostName = forum.UrlHostName,
					ForumIdentifier = forum.Identifier,
					Moderated = forum.Moderated,
					Private = forum.Private,
				};

			return View(model);
		}
	}
}