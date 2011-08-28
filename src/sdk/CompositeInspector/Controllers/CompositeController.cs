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

            ViewBag.CompositeName = _composite.Name;
            ViewBag.CompositeDescription = _composite.Description;
		}

		public ViewResult Index()
		{
			return
				View(
					new CompositeModel
						{
							Name = _composite.Name,
							Description = _composite.Description,
							Agents = _composite.Agents,
							InputModels = _composite.InputModels,
							ConfigurationErrors = _composite.GetConfigurationErrors(),
							Settings = _composite.Settings
						});
		}
	}
}