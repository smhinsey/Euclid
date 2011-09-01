using System;
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
			try
			{
				var zero = 0;
				var dumb = 9/zero;
			}
			catch (Exception e)
			{
				_logger.WriteErrorMessage("An error occurred", e);
			}

			_logger.WriteDebugMessage("Test debug message");

			

			return View();
		}
	}
}