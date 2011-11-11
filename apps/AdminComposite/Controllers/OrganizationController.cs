using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminComposite.Models;

namespace AdminComposite.Controllers
{
	public class OrganizationController : Controller
	{
		//
		// GET: /Organization/
		public ActionResult Details(Guid organizationId)
		{
			ViewBag.Title = string.Format("Manage Organization {0}", organizationId);
			return View();
		}

		public ActionResult ListUsers(Guid organizationId)
		{
			return View();
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
