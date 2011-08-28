using System.Web.Mvc;

namespace CompositeInspector.Controllers
{
    public class FrameController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
         
        public ActionResult Remove()
        {
            return RedirectToAction("Index", "Composite");
        }
    }
}