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
			var model = new AllCategoriesViewModel { Categories = _categoryQueries.FindCategoriesForForum(ForumIdentifier, 5) };

			return View(model);
		}

		public ActionResult Detail(string categorySlug)
		{
			var model = new CategoryDetailsViewModel();

			var category = _categoryQueries.FindBySlug(ForumIdentifier, categorySlug);

			model.Name = category.Name;
			model.Listing = _postQueries.GetPostListingByCategory(ForumIdentifier, categorySlug, 22, 0);

			return View(model);
		}
	}
}