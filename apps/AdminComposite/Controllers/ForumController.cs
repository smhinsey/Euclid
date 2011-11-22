using System;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class ForumController : Controller
	{
		private readonly ForumQueries _forumQueries;
		private readonly IPublisher _publisher;

		public ForumController(ForumQueries forumQueries, IPublisher publisher)
		{
			_forumQueries = forumQueries;
			_publisher = publisher;
			AutoMapper.Mapper.CreateMap<Forum, UpdateForumInputModel>().ForMember(input=>input.ForumIdentifier, opt=>opt.MapFrom(forum=>forum.Identifier));
		}

		public ActionResult AuthenticationProviders(Guid forumId)
		{
			ViewBag.ForumName = _forumQueries.FindById(forumId).Name;
			return View();
		}

		public ActionResult Create()
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

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
			var model = AutoMapper.Mapper.Map<UpdateForumInputModel>(forum);

			return View(model);
		}

		public JsonResult SetForumTheme(Guid forumId, string theme)
		{
			var publicationId = _publisher.PublishMessage(new SetForumTheme
			                                              	{
			                                              		ForumIdentifier = forumId,
			                                              		ThemeName = theme
			                                              	});

			return Json(new {publicationId}, JsonRequestBehavior.AllowGet);
		}
	}
}