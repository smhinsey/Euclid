
d:\Projects\Euclid\platform>@git.exe %*
﻿using System;
using System.Web.Mvc;
using ForumAgent.Queries;
using ForumComposite.Models;

namespace ForumComposite.Controllers
{
	public class PostController : Controller
	{
		private readonly PostQueries _postQueries;

		private readonly CommentQueries _commentQueries;

		public PostController(PostQueries postQueries, CommentQueries commentQueries)
		{
			_postQueries = postQueries;
			_commentQueries = commentQueries;
		}

		public ActionResult AddComment()
		{
			return View(new CommentOnPostInputModel());
		}

		public ActionResult Create()
		{
			return View(new PublishPostInputModel());
		}

		public ActionResult List()
		{
			var posts = _postQueries.FindByCreationDate(DateTime.Now.AddDays(-5), DateTime.Now);

			return View(posts);
		}

		public ActionResult Thread()
		{
			var comments = _commentQueries.FindCommentsBelongingToPost(Guid.NewGuid());

			return View(comments);
		}
	}
}
d:\Projects\Euclid\platform>@set ErrorLevel=%ErrorLevel%

d:\Projects\Euclid\platform>@rem Restore the original console codepage.

d:\Projects\Euclid\platform>@chcp %cp_oem% > nul < nul
