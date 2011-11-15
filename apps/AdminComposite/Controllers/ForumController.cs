using System;
using System.Web.Mvc;
using AdminComposite.Models;
using AutoMapper;
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
			Mapper.CreateMap<Forum, CreateForumInputModel>();
		}

		public ActionResult AuthenticationProviders(Guid forumId)
		{
			return View();
		}

		public ActionResult Create()
		{
			return View(new CreateForumInputModel { UrlHostName = "socialrally.com" });
		}

		public ActionResult Details(Guid forumId)
		{
			var forum = _forumQueries.FindById(forumId);
			var model = Mapper.Map<CreateForumInputModel>(forum);

			return View(model);
		}
	}
}