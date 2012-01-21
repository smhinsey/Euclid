using System.Web.Mvc;
using Euclid.Common.Logging;

namespace AdminComposite.Controllers
{
	[Authorize]
	public class DashboardController : AdminController, ILoggingSource
	{
		public PartialViewResult GetConfirmationMessage(string message)
		{
			return PartialView("GetConfirmationMessage", message);
		}

		public PartialViewResult GetConfirmationMessageAttributesMissingMessage()
		{
			return PartialView();
		}

		public ActionResult Index()
		{
			this.WriteInfoMessage("Loaded dashboard.");

			return View();
		}
	}
}