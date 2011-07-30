using System.Web.Mvc;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.Controllers
{
	public class AgentController : Controller
	{
		public ViewResult Operations(IAgentInfo info)
		{
			return View(info);
		}
	}
}