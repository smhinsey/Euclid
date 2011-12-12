using System;
using System.Web.Mvc;
using AdminComposite.Extensions;
using AdminComposite.Models;
using ForumAgent;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class ForumController : Controller
	{
		private readonly ForumQueries _forumQueries;
		
		public ForumController(ForumQueries forumQueries)
		{
			_forumQueries = forumQueries;
		}

		public ActionResult AuthenticationProviders(Guid forumId)
		{
			ViewBag.ForumName = _forumQueries.FindById(forumId).Name;
			return View();
		}

		public ActionResult Create()
		{
			var userId = Request.GetLoggedInUserId();

			return View(new CreateForumInputModel
			            	{
			            		UrlHostName = "newco-forums.com",
			            		OrganizationId = ViewBag.OrganizationId,
			            		Description = " ",
								CreatedBy = userId,
								VotingScheme = VotingScheme.UpDownVoting,
								Theme = "Swiss"
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