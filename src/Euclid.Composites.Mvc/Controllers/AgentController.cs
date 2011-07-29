using System.Web.Mvc;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.Controllers
{
	public class AgentController : Controller
	{
		public ViewResult Operations(IAgentMetadata metadata)
		{
			if (metadata == null)
			{
				ViewBag.Header = "Could not find agent metadata";
			}
			else
			{
				ViewBag.Header = "Found metadataa for agent: " + metadata.FriendlyName;
			}

			return View(metadata);
		}
	}
}