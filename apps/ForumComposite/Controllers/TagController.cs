using System.Collections.Generic;
using System.Web.Mvc;
using ForumAgent.Queries;

namespace ForumComposite.Controllers
{
	public class TagController : ForumController
	{
		private readonly TagQueries _tagQueries;

		public TagController(TagQueries tagQueries)
		{
			_tagQueries = tagQueries;
		}

		public ActionResult All()
		{
			return View();
		}

		public ActionResult Detail()
		{
			return View();
		}
	}
}