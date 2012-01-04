using System.Web.Mvc;
using ForumAgent.Queries;
using ForumComposite.ViewModels.Category;

namespace ForumComposite.Controllers
{
	public class CategoryController : ForumController
	{
		private readonly CategoryQueries _categoryQueries;

		private readonly PostQueries _postQueries;

		public CategoryController(CategoryQueries categoryQueries, PostQueries postQueries)
		{
			_categoryQueries = categoryQueries;
			_postQueries = postQueries;
		}

		public ActionResult All()
		{
			var model = new AllCategoriesViewModel { Categories = _categoryQueries.FindCategoriesForForum(ForumInfo.ForumIdentifier, 5) };

			return View(model);
		}

		public ActionResult Detail(string categorySlug, int? page)
		{
			var offset = page.GetValueOrDefault(1);

			offset--;

			offset = offset * 16;

			var model = new CategoryDetailsViewModel
				{
					CurrentPage = page.GetValueOrDefault(1),
					Listing = _postQueries.FindPostsInCategory(ForumInfo.ForumIdentifier, categorySlug, 16, offset)
				};

			var category = _categoryQueries.FindBySlug(ForumInfo.ForumIdentifier, categorySlug);

			model.Name = category.Name;

			return View(model);
		}
	}
}