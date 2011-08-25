using System.Web.Mvc;
using Euclid.Composites;
using PortableAgentPanel.Models;

namespace PortableAgentPanel.Controllers
{
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