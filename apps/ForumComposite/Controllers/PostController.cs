using System;
using System.Web.Mvc;
using ForumAgent.Queries;
using ForumComposite.Models;

namespace ForumComposite.Controllers
{
	public class PostController : Controller
	{
		private readonly PostQueries _postQueries;

		public PostController(PostQueries postQueries)
		{
			_postQueries = postQueries;
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

			return View();
		}

		public ActionResult Thread()
		{
			var postAndComments = _postQueries.FindByIdWithComments(Guid.Empty);

			return View();
		}
	}
}