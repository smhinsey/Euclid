using System.Collections.Generic;
using System.Web.Mvc;
using ForumAgent.Queries;
using ForumComposite.ViewModels.Tag;

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
			var model = new AllTagsViewModel { Tags = _tagQueries.FindTagsForForum(ForumInfo.ForumIdentifier, 5) };

			return View(model);
		}

		public ActionResult Detail()
		{
			return View();
		}
	}
}