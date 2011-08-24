using System;
using System.Web.Mvc;
using Euclid.Framework.Cqrs;

namespace CompositeControl.Areas.CompositeControl.Controllers
{
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

			return View(record);
		}

		public ActionResult Index(int pageSize = 100, int offset = 0)
		{
			var records = _registry.GetRecords(pageSize, offset);

			return View(new { PublicationRecords = records });
		}
	}
}