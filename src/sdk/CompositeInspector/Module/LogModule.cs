using System;
using System.Linq;
using Euclid.Composites;
using JsonCompositeInspector.Models;
using LoggingAgent.Queries;
using Nancy;

namespace JsonCompositeInspector.Module
{
	public class LogModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;
		private readonly LogQueries _logQueries;

		public LogModule(ICompositeApp compositeApp, LogQueries logQueries)
			: base("composite/logs")
		{
			_compositeApp = compositeApp;
			_logQueries = logQueries;

			Get["/"] = p =>
							{
								var filterBy = (string)p.filterBy ?? string.Empty;
								var pageSize = p.pageSize != null ? (int)p.pageSize : 25;
								var offset = p.offset != null ? (int)p.offset : 0;

								var startDate = DateTime.Now.AddMinutes(-15);
								var endDate = DateTime.Now;
								var entries = _logQueries.FindByCreationDate(startDate, endDate);
								var sources = entries.Select(e => e.LoggingSource).Distinct();

								if (!string.IsNullOrEmpty(filterBy))
								{
									entries = entries.Where(e => e.LoggingSource == filterBy).ToList();
								}

								var model = new LogModel
												{
													AvailableSources = sources,
													Entries = entries.Skip(offset * pageSize).Take(pageSize).OrderByDescending(e => e.Created),
													SelectedSource = filterBy
												};

								return View["logs.cshtml", model];
							};
		}
	}
}