using System.Web.Mvc;
using Euclid.Common.Logging;

namespace Euclid.Sdk.TestComposite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILoggingSource _logger;

		public HomeController(ILoggingSource logger)
		{
			_logger = logger;
		}

		public ActionResult Index()
		{
			_logger.WriteDebugMessage("Test debug message");

			return View();
		}
	}
}