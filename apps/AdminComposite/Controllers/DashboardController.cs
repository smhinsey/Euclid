﻿using System.Web.Mvc;
using Euclid.Common.Logging;

namespace AdminComposite.Controllers
{
	public class DashboardController : Controller, ILoggingSource
	{
		public ActionResult Index()
		{
			this.WriteInfoMessage("Loaded dashboard.");

			return View();
		}
	}
}