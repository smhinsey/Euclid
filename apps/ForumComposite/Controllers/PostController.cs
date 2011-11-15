using System;
using System.Web.Mvc;
using Euclid.Common.Logging;
using ForumAgent.Queries;
using ForumComposite.Models;

namespace ForumComposite.Controllers
{
	[AddUserSpecificData()]
	public class PostController : Controller, ILoggingSource
	{
		private readonly CommentQueries _commentQueries;

		private readonly PostQueries _postQueries;

		public PostController(PostQueries postQueries, CommentQueries commentQueries)
		{
			_postQueries = postQueries;
			_commentQueries = commentQueries;
		}

		public ActionResult AddComment(Guid postId)
		{
			// SELF there's a better way to do this, but i'm lazy
			var authorId = Guid.Parse(Request.Cookies["ForumUserId"].Value);

			this.WriteInfoMessage("Loading add comment screen for user {0}({1}).", User.Identity.Name, authorId);

			return View(new CommentOnPostInputModel { PostIdentifier = postId, AuthorIdentifier = authorId });
		}

		public ActionResult Create()
		{
			var authorId = Guid.Parse(Request.Cookies["ForumUserId"].Value);

			return View(new PublishPostInputModel { AuthorIdentifier = authorId });
		}

		public ActionResult List(int pageSize = 10, int offset = 0)
		{
			var posts = _postQueries.GetPostListing(ViewBag.ForumIdentifier, pageSize, offset);

			return View(posts);
		}

		public ActionResult Thread(Guid postId)
		{
			var comments = _commentQueries.FindCommentsBelongingToPost(ViewBag.ForumIdentifier, postId);

			return View(comments);
		}
	}
}