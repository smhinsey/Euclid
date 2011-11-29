﻿using System;
using System.Web.Mvc;
using AdminComposite.Models;
using Euclid.Common.Messaging;
using ForumAgent;
using ForumAgent.Commands;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class CategoryController : Controller
	{
		private readonly CategoryQueries _categoryQueries;
		private readonly IPublisher _publisher;

		public CategoryController(CategoryQueries categoryQueries, ForumQueries forumQueries, IPublisher publisher)
		{
			_categoryQueries = categoryQueries;
			_publisher = publisher;
		}

		public ActionResult List(Guid forumId, int offset = 0, int pageSize = 25)
		{
			var model = _categoryQueries.List(forumId, offset, pageSize);
			ViewBag.ForumName = model.ForumName;

			ViewBag.Pagination = new PaginationModel
			                     	{
			                     		ActionName = "List",
			                     		ControllerName = "Category",
			                     		ForumIdentifier = forumId,
			                     		PageSize = pageSize,
			                     		Offset = offset,
			                     		TotalItems = model.TotalCategories
			                     	};

			return View(model);
		}

		public PartialViewResult NewCategory(Guid forumId)
		{
			var userId = Guid.Parse(Request.Cookies["OrganizationUserId"].Value);

			return PartialView("_NewCategory", new CreateCategoryInputModel
			                                   	{
			                                   		ForumIdentifier = forumId, 
													CreatedBy = userId
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
			                                      		CategoryIdentifier = categoryId,
			                                      		Name = category.Name,
			                                      		Active = category.Active
			                                      	});
		}

		public JsonResult ActivateCategory(Guid categoryId, bool active)
		{
			var publicationId = _publisher.PublishMessage(new ActivateCategory
			                                              	{
			                                              		CategoryIdentifier = categoryId,
			                                              		Active = active
			                                              	});

			return Json(new {publicationId}, JsonRequestBehavior.AllowGet);
		}
	}
}