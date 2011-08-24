using System;
using System.Web.Mvc;
using ForumAgent.Queries;

namespace ForumComposite.Controllers
{
	public class PostController : Controller
	{
		private readonly PostQueries _postQueries;

		public PostController(PostQueries postQueries)
		{
			_postQueries = postQueries;
		}

		public ActionResult List()
		{
			var posts = _postQueries.FindByCreationDate(DateTime.Now.AddDays(-5), DateTime.Now);

			return View();
		}

		public ActionResult Thread()
		{
			return View();
		}

		public ActionResult Create()
		{
			return View();
		}
	}
}