using System;
using System.Web.Mvc;
using AdminComposite.Models;
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
			AutoMapper.Mapper.CreateMap<Forum, CreateForumInputModel>();
		}

		public ActionResult Create()
		{
			return View(new CreateForumInputModel {UrlHostName = "socialrally.com"});
		}

		public ActionResult AuthenticationProviders(Guid forumId)
		{
			return View();
		}

		public ActionResult Details(Guid forumId)
		{
			var forum = _forumQueries.FindById(forumId);
			var model = AutoMapper.Mapper.Map<CreateForumInputModel>(forum);

			return View(model);
		}
	}
}