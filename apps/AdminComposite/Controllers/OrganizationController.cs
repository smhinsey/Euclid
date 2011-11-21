using System;
using System.Web.Mvc;
using AdminComposite.Models;
using AutoMapper;
using ForumAgent;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class OrganizationController : Controller
	{
		private readonly OrganizationQueries _organizationQueries;

		private readonly OrganizationUserQueries _userQueries;

		public OrganizationController(OrganizationUserQueries userQueries, OrganizationQueries organizationQueries)
		{
			_userQueries = userQueries;
			_organizationQueries = organizationQueries;
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

		public ActionResult Users(Guid organizationId, int pageNumber = 0, int pageSize = 25)
		{
			ViewBag.OrganizationName = _organizationQueries.FindById(organizationId).Name;

			return View(_userQueries.FindByOrganization(organizationId, pageNumber * pageSize, pageSize));
		}
	}
}