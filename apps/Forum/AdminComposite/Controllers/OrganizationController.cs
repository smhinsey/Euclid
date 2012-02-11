using System;
using System.Web.Mvc;
using AdminComposite.Areas.Forum.InputModels;
using AdminComposite.InputModels;
using ForumAgent;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class OrganizationController : AdminController
	{
		private readonly OrganizationQueries _organizationQueries;
		private readonly OrganizationUserQueries _userQueries;

		public OrganizationController(OrganizationUserQueries userQueries, OrganizationQueries organizationQueries)
		{
			_userQueries = userQueries;
			_organizationQueries = organizationQueries;
		}

		//
		// GET: /Organization/
		public ActionResult Details(Guid organizationId)
		{
			ViewBag.Title = string.Format("Manage Organization {0}", organizationId);
			var org = _organizationQueries.FindById(organizationId);

			return View(
				new UpdateOrganizationInputModel
					{
						Address = org.Address,
						Address2 = org.Address2,
						City = org.City,
						Country = org.Country,
						OrganizationIdentifier = org.Identifier,
						OrganizationName = org.Name,
						OrganizationSlug = org.Slug,
						PhoneNumber = org.PhoneNumber,
						OrganizationUrl = org.WebsiteUrl,
						State = org.State,
						Zip = org.Zip
					});
		}

		[HttpGet]
		public PartialViewResult RegisterUser(Guid organizationId, Guid currentUserId)
		{
			return PartialView(
				"_RegisterOrganizationUser",
				new RegisterOrganizationUserInputModel { OrganizationId = organizationId, CreatedBy = currentUserId });
		}

		[HttpGet]
		public PartialViewResult UpdateUser(Guid organizationId, Guid userId)
		{
			var user = _userQueries.FindById(userId);

			if (user == null)
			{
				throw new UserNotFoundException(userId);
			}

			return PartialView("_UpdateOrganizationUser", new UpdateOrganizationUserInputModel
															{
																Email = user.Email,
																FirstName = user.FirstName,
																LastName = user.LastName,
																UserId = user.Identifier,
																OrganizationId = user.OrganizationIdentifier,
																Username = user.Username,
															});
		}

		public ActionResult Users(Guid organizationId, int offset = 0, int pageSize = 25)
		{
			var model = _userQueries.FindByOrganization(organizationId, offset, pageSize);
			ViewBag.Pagination = new PaginationModel
									{
										ActionName = "Users",
										ControllerName = "Organization",
										IdentifierParameterName = "organizationId",
										Identifier = model.OrganizationIdentifier,
										Offset = offset,
										PageSize = pageSize,
										TotalItems = model.TotalNumberOfUsers
									};

			return View(model);
		}
	}
}