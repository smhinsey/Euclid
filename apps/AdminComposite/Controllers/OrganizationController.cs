using System;
using System.Web.Mvc;
using AdminComposite.Models;
using ForumAgent.Processors;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class OrganizationController : Controller
	{
		private readonly OrganizationUserQueries _userQueries;
		private readonly OrganizationQueries _organizationQueries;

		public OrganizationController(OrganizationUserQueries userQueries, OrganizationQueries organizationQueries)
		{
			_userQueries = userQueries;
			_organizationQueries = organizationQueries;
			AutoMapper.Mapper.CreateMap<OrganizationUser, UpdateOrganizationUserInputModel>();
			AutoMapper.Mapper.CreateMap<Organization, UpdateOrganizationInputModel>()
				.ForMember(input => input.OrganizationName, o => o.MapFrom(org => org.Name))
				.ForMember(input => input.OrganizationUrl, o => o.MapFrom(org => org.WebsiteUrl))
				.ForMember(input => input.OrganizationSlug, o=>o.MapFrom(org=>org.Slug))
				.ForMember(input => input.OrganizationIdentifier, o=>o.MapFrom(org=>org.Identifier));
		}

		//
		// GET: /Organization/
		public ActionResult Details(Guid organizationId)
		{
			ViewBag.Title = string.Format("Manage Organization {0}", organizationId);
			var org = _organizationQueries.FindById(organizationId);

			return View(AutoMapper.Mapper.Map<UpdateOrganizationInputModel>(org));
		}

		public ActionResult Users(Guid organizationId, int pageNumber = 0, int pageSize = 25)
		{
			return View(_userQueries.List(pageNumber * pageSize, pageSize));
		}

		[HttpGet]
		public PartialViewResult RegisterUser(Guid organizationId, Guid currentUserId)
		{
			return PartialView("_RegisterOrganizationUser", new RegisterOrganizationUserInputModel
			                                                	{
			                                                		OrganizationId = organizationId,
																	CreatedBy = currentUserId
			                                                	});
		}

		[HttpGet]
		public PartialViewResult UpdateUser(Guid organizationId, Guid userId)
		{
			var user = _userQueries.FindById(userId);

			if (user == null)
			{
				throw new UserNotFoundException(userId);
			}

			var model = AutoMapper.Mapper.Map<UpdateOrganizationUserInputModel>(user);
			
			return PartialView("_UpdateOrganizationUser", model);
		}
	}
}
