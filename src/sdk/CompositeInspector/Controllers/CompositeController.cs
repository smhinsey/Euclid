using System.Web.Mvc;
using CompositeInspector.Models;
using Euclid.Composites;
using MvcContrib.Filters;

namespace CompositeInspector.Controllers
{
    [Layout("_Layout")]
    public class CompositeController : Controller
    {
        private readonly ICompositeApp _composite;

        public CompositeController(ICompositeApp composite)
        {
            _composite = composite;
        }

        public ViewResult Index()
        {
            ViewBag.Title = "Composite Metadata";

            return
                View(
                    new CompositeModel
                        {
                            Agents = _composite.Agents,
                            InputModels = _composite.InputModels,
                            ConfigurationErrors = _composite.GetConfigurationErrors(),
                            Settings = _composite.Settings
                        });
        }
    }
}