using System.Web.Mvc;
using Euclid.Common.Logging;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class DashboardController : Controller, ILoggingSource
	{
		public ActionResult Index()
		{
			this.WriteInfoMessage("Loaded dashboard.");

			return View();
		}

		public PartialViewResult GetConfirmationMessageAttributesMissingMessage()
		{
			return PartialView();
		}

		public PartialViewResult GetConfirmationMessage(string message)
		{
			return PartialView("GetConfirmationMessage", message);
		}
	}
}