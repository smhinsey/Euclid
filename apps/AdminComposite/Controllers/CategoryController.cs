using System;
using System.Web.Mvc;
using AdminComposite.Models;
using ForumAgent;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class CategoryController : Controller
	{
		private readonly CategoryQueries _categoryQueries;

		public CategoryController(CategoryQueries categoryQueries)
		{
			_categoryQueries = categoryQueries;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			return View(_categoryQueries.List(offset, pageSize));
		}

		public PartialViewResult NewCategory(Guid forumId)
		{
			return PartialView("_NewCategory", new CreateCategoryInputModel
			                                   	{
			                                   		ForumIdentifier = forumId, 
													CreatedBy = ViewBag.UserId
			                                   	});
		}

		public PartialViewResult UpdateCategory(Guid categoryId)
		{
			var category = _categoryQueries.FindById(categoryId);

			if (category == null)
			{
				throw new CategoryNotFoundException(string.Format("Could not find the category with id {0}", categoryId));
			}

			return PartialView("_UpdateCategory", new UpdateCategoryInputModel
			                                      	{
			                                      		Identifier = categoryId,
			                                      		Name = category.Name,
			                                      		Active = category.Active
			                                      	});
		}
	}
}