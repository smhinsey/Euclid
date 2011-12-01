using System;
using System.Web.Mvc;
using AdminComposite.Models;
using AutoMapper;
using Euclid.Common.Messaging;
using ForumAgent;
using ForumAgent.Commands;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class OrganizationController : Controller
	{
		private readonly OrganizationQueries _organizationQueries;

		private readonly OrganizationUserQueries _userQueries;

		private readonly IPublisher _publisher;

		public OrganizationController(OrganizationUserQueries userQueries, OrganizationQueries organizationQueries, IPublisher publisher)
		{
			_userQueries = userQueries;
			_organizationQueries = organizationQueries;
			_publisher = publisher;
			Mapper.CreateMap<OrganizationUser, UpdateOrganizationUserInputModel>();
			Mapper.CreateMap<Organization, UpdateOrganizationInputModel>().ForMember(
				input => input.OrganizationName, o => o.MapFrom(org => org.Name)).ForMember(
					input => input.OrganizationUrl, o => o.MapFrom(org => org.WebsiteUrl)).ForMember(
						input => input.OrganizationSlug, o => o.MapFrom(org => org.Slug)).ForMember(
							input => input.OrganizationIdentifier, o => o.MapFrom(org => org.Identifier));
		}

		//
		// GET: /Organization/
		public ActionResult Details(Guid organizationId)
		{
			ViewBag.Title = string.Format("Manage Organization {0}", organizationId);
			var org = _organizationQueries.FindById(organizationId);

			return View(Mapper.Map<UpdateOrganizationInputModel>(org));
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

			var model = Mapper.Map<UpdateOrganizationUserInputModel>(user);

			return PartialView("_UpdateOrganizationUser", model);
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

		public JsonResult ActivateUser(Guid userId, bool activate)
		{
			var publicationRecordId = _publisher.PublishMessage(new ActivateOrganizationUser
			                          	{
			                          		UserIdentifier = userId,
			                          		Active = activate
			                          	});

			return Json(new {publicationRecordId}, JsonRequestBehavior.AllowGet);
		}
	}
}