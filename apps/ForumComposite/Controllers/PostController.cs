using System;
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

		public ActionResult List(int pageSize = 10, int offset = 0)
		{
			var posts = _postQueries.GetPostListing(pageSize, offset);

			return View(posts);
		}

		public ActionResult Thread(Guid postId)
		{
			var comments = _commentQueries.FindCommentsBelongingToPost(postId);

			return View(comments);
		}
	}
}
