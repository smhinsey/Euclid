﻿using System.Web.Mvc;
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
			return View();
		}

		public ActionResult Detail(string categorySlug)
		{
			var model = new CategoryDetailsViewModel();

			var category = _categoryQueries.FindBySlug(ForumIdentifier, categorySlug);

			model.Name = category.Name;
			model.Posts = _postQueries.GetPostListingByCategory(ForumIdentifier, categorySlug, 20, 0);

			return View(model);
		}
	}
}