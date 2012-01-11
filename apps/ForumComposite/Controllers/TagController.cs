using System.Web.Mvc;
using ForumAgent.Queries;
using ForumComposite.ViewModels.Tag;

namespace ForumComposite.Controllers
{
	public class TagController : ForumController
	{
		private readonly PostQueries _postQueries;

		private readonly TagQueries _tagQueries;

		public TagController(TagQueries tagQueries, PostQueries postQueries)
		{
			_tagQueries = tagQueries;
			_postQueries = postQueries;
		}

		public ActionResult All()
		{
			var model = new AllTagsViewModel { Tags = _tagQueries.FindTagsForForum(ForumInfo.ForumIdentifier, 5) };

			return View(model);
		}

		public ActionResult Detail(string tagSlug, int? page)
		{
			var offset = page.GetValueOrDefault(1);

			offset--;

			offset = offset * 16;

			var model = new TagDetailViewModel
				{
					Name = tagSlug,
					Listing = _postQueries.FindPostsInTag(ForumInfo.ForumIdentifier, tagSlug, 16, offset)
				};

			return View(model);
		}
	}
}