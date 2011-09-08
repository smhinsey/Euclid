using System;
using System.Web.Mvc;
using ForumAgent.Queries;

namespace ForumComposite.Controllers
{
	[AddUserSpecificData]
	public class ProfileController : Controller
	{
		private readonly PostQueries _postQueries;

		public ProfileController(PostQueries postQueries)
		{
			_postQueries = postQueries;
		}

		public ActionResult MyDiscussions(int pageSize = 10, int offset = 0)
		{
			// SELF there's a better way to do this, but i'm lazy
			var authorId = Guid.Parse(Request.Cookies["ForumUserId"].Value);

			var posts = _postQueries.FindByAuthorIdentifier(authorId, pageSize, offset);

			return View(posts);
		}
	}
}