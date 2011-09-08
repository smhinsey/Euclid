using System.Web.Mvc;
using Euclid.Composites;

namespace CompositeInspector.Controllers
{
	public class FrameController : Controller
	{
		public FrameController(ICompositeApp composite)
		{
			ViewBag.CompositeName = composite.Name;
			ViewBag.CompositeDescription = composite.Description;
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Remove()
		{
			return Redirect(Url.Content("~"));
		}
	}
}