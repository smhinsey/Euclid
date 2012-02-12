using System;
using System.Web.Mvc;
using AdminComposite.Areas.Forum.InputModels;
using AdminComposite.Controllers;
using ForumAgent;
using ForumAgent.Queries;

namespace AdminComposite.Areas.Forum.Controllers
{
	[Authorize]
	public class CategoryController : AdminController
	{
		private readonly CategoryQueries _categoryQueries;

		public CategoryController(CategoryQueries categoryQueries, ForumQueries forumQueries)
		{
			_categoryQueries = categoryQueries;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			var model = _categoryQueries.List(forumId, offset, pageSize);
			ViewBag.Pagination = new PaginationModel
				{
					ActionName = "List",
					ControllerName = "Category",
					Identifier = forumId,
					PageSize = pageSize,
					Offset = offset,
					TotalItems = model.TotalCategories
				};

			return View(model);
		}

		public PartialViewResult NewCategory(Guid forumId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			return PartialView("_NewCategory", new CreateCategoryInputModel { ForumIdentifier = forumId, CreatedBy = userId });
		}

		public PartialViewResult UpdateCategory(Guid categoryId)
		{
			var category = _categoryQueries.FindById(categoryId);

			if (category == null)
			{
				throw new CategoryNotFoundException(string.Format("Could not find the category with id {0}", categoryId));
			}

			return PartialView(
				"_UpdateCategory",
				new UpdateCategoryInputModel { CategoryIdentifier = categoryId, Name = category.Name, Active = category.Active });
		}
	}
}