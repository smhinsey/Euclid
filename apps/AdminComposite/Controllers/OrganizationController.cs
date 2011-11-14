using System;
using System.Web.Mvc;
using AdminComposite.Models;
using ForumAgent.Queries;
using ForumAgent.ReadModels;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class OrganizationController : Controller
	{
		private readonly OrganizationUserQueries _userQueries;

		public OrganizationController(OrganizationUserQueries userQueries)
		{
			_userQueries = userQueries;
			AutoMapper.Mapper.CreateMap<OrganizationUser, RegisterOrganizationUserInputModel>();
		}

		//
		// GET: /Organization/
		public ActionResult Details(Guid organizationId)
		{
			ViewBag.Title = string.Format("Manage Organization {0}", organizationId);
			return View();
		}

		public ActionResult ListUsers(Guid organizationId, int pageNumber = 0, int pageSize = 25)
		{
			return View(_userQueries.List(pageNumber * pageSize, pageSize));
		}

		[HttpGet]
		public PartialViewResult RegisterUser(Guid organizationId, Guid? userId)
		{
			ViewBag.Title = "Add a user";

			RegisterOrganizationUserInputModel model;

			if (!userId.HasValue)
			{
				model = new RegisterOrganizationUserInputModel
				        	{
				        		OrganizationId = organizationId
				        	};
			}
			else
			{
				model = AutoMapper.Mapper.Map<RegisterOrganizationUserInputModel>(_userQueries.FindByIdentifier(userId.Value));
			}


			return PartialView("_OrganizationUserForm", model);
		}
	}
}
