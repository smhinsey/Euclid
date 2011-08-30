using System.Web.Mvc;
using LoggingAgent.Queries;

namespace CompositeInspector.Controllers
{
	public class LoggingController : Controller
	{
		private readonly LogQueries _logQueries;

		public LoggingController(LogQueries logQueries)
		{
			_logQueries = logQueries;
		}

		public ActionResult Index(int pageSize = 100, int offset = 0)
		{
			return View(_logQueries.GetLogEntries(pageSize, offset));
		}
	}
}