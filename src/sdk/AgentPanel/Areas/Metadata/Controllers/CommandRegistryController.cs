using System.Web.Mvc;
using System.Web.UI.WebControls;
using Euclid.Framework.Cqrs;

namespace AgentPanel.Areas.Metadata.Controllers
{
    public class CommandRegistryController : Controller
    {
        private readonly ICommandRegistry _registry;

        public CommandRegistryController(ICommandRegistry registry)
        {
            _registry = registry;
        }

        public ActionResult Index()
        {
            var records = _registry.GetRecords(999, 0);

            return View(new
                            {
                                PublicationRecordId = records[0].Identifier,
                                PublicationRecords = records
                            });
        }
    }
}