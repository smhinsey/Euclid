using System;
using System.Web.Mvc;
using AdminComposite.Models;
using ForumAgent.Queries;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class OrganizationController : Controller
	{
		private readonly OrganizationUserQueries _userQueries;

		public OrganizationController(OrganizationUserQueries userQueries)
		{
			_userQueries = userQueries;
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
		public PartialViewResult GetUser(Guid? organizationId, Guid? userId)
		{
			ViewBag.Title = "Add a user";

			if (!organizationId.HasValue)
			{
				organizationId = Guid.Empty;
			}

			if (!userId.HasValue)
			{
				userId = Guid.Empty;
			}

			return PartialView("_OrganizationUserForm", new CreateOrganizationUserModel
			                                           	{
			                                           		ContactInfoRequired = false,
															OrganizationIdentifier = organizationId.Value,
															DisplayTitle = false,
			                                           	});
		}


	}
}
