using System.Web.Mvc;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.Controllers
{
	public class AgentController : Controller
	{
		public ViewResult Operations(IAgentInfo info)
		{
			if (info != null)
			{
				ViewBag.Header = "Found metadataa for agent: " + info.FriendlyName;
			}

			return View(info);
		}
	}
}