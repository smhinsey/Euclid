﻿using System;
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
			AutoMapper.Mapper.CreateMap<Forum, UpdateForumInputModel>().ForMember(input=>input.ForumIdentifier, opt=>opt.MapFrom(forum=>forum.Identifier));
		}

		public ActionResult AuthenticationProviders(Guid forumId)
		{
			return View();
		}

		public ActionResult Create()
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			return View(new CreateForumInputModel
			            	{
			            		UrlHostName = "socialrally.com",
			            		OrganizationId = ViewBag.OrganizationId,
			            		Description = " ",
								CreatedBy = userId,
								VotingScheme = VotingScheme.UpDownVoting
			            	});
		}

		public ActionResult Details(Guid forumId)
		{
			var forum = _forumQueries.FindById(forumId);
			var model = AutoMapper.Mapper.Map<UpdateForumInputModel>(forum);

			return View(model);
		}
	}
}