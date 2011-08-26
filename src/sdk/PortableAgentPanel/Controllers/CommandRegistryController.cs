using System;
using System.Web.Mvc;
using CompositeInspector.Models;
using Euclid.Framework.Cqrs;
using MvcContrib.Filters;

namespace CompositeInspector.Controllers
{
    [Layout("_Layout")]
    public class CommandRegistryController : Controller
    {
        private readonly ICommandRegistry _registry;

        public CommandRegistryController(ICommandRegistry registry)
        {
            _registry = registry;
        }

        public ActionResult Details(Guid publicationId)
        {
            var record = _registry.GetPublicationRecord(publicationId);

            return View(new CommandPublicationModel
                            {
                                HasValue = record != null,
                                Record = record,
                                PublicationId = publicationId.ToString()
                            });
        }

        public ActionResult Index(int pageSize = 100, int offset = 0)
        {
            var records = _registry.GetRecords(pageSize, offset);

            return View(new {PublicationRecords = records});
        }
    }
}