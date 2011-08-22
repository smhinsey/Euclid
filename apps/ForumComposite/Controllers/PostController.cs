using System.Web.Mvc;

namespace ForumComposite.Controllers
{
	public class PostController : Controller
	{
		// GET: /Home/
		public ActionResult List()
		{
			return this.View();
		}

		public ActionResult Thread()
		{
			return this.View();
		}
	}
}