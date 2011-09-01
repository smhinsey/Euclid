using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using CompositeInspector.Models;
using Euclid.Composites.Mvc.Extensions;
using LoggingAgent.Queries;
using MvcContrib.Filters;

namespace CompositeInspector.Controllers
{
	[Layout("_Layout")]
 	public class LoggingController : Controller
	{
		private readonly LogQueries _logQueries;

		public LoggingController(LogQueries logQueries)
		{
			_logQueries = logQueries;
		}

		public ActionResult Index(int pageSize = 100, int offset = 0, string filterBy = null)
		{
			var startDate = DateTime.Now.AddMinutes(-15);
			var endDate = DateTime.Now;
			var entries = _logQueries.FindByCreationDate(startDate, endDate);
			var sources = entries.Select(e => e.LoggingSource).Distinct();

			if (!string.IsNullOrEmpty(filterBy))
			{
				entries = entries.Where(e => e.LoggingSource == filterBy).ToList();
			}

			entries.SetupPaging(this, pageSize, offset);

			ViewBag.Title = "Log Entries";

			return View(new LogEntryModel
			            	{
			            		Entries = entries.Skip(offset * pageSize).Take(pageSize).OrderByDescending(e=>e.Created),
								AvailableSources = sources,
								SelectedSource = filterBy
			            	});
		}
	}
}