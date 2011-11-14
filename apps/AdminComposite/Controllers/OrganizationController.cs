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

		public OrganizationController(OrganizationUserQueries userQueries)
		{
			_userQueries = userQueries;
			AutoMapper.Mapper.CreateMap<OrganizationUser, UpdateOrganizationUserInputModel>();
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
			var user = _userQueries.FindByIdentifier(userId);

			if (user == null)
			{
				throw new UserNotFoundException(userId);
			}

			var model = AutoMapper.Mapper.Map<UpdateOrganizationUserInputModel>(user);
			
			return PartialView("_UpdateOrganizationUser", model);
		}
	}
}
