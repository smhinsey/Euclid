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
		public ActionResult Index()
		{
			ViewBag.Title = string.Format("Manage Organization {0}", Guid.NewGuid());
			return View();
		}

		[HttpGet]
		public PartialViewResult GetNewOrganizationUser()
		{
			ViewBag.Title = "Add a user";
			return PartialView("_NewOrganizationUser", new CreateOrganizationUserModel
			                                           	{
			                                           		ContactInfoRequired = false,
			                                           		OrganizationIdentifier = Guid.NewGuid(),
															DisplayTitle = false,
			                                           	});
		}
	}
}
