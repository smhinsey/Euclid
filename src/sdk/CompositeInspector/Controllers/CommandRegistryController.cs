using System;
using System.Web.Mvc;
using CompositeInspector.Models;
using Euclid.Composites;
using Euclid.Framework.Cqrs;
using MvcContrib.Filters;

namespace CompositeInspector.Controllers
{
	[Layout("_Layout")]
	public class CommandRegistryController : Controller
	{
		private readonly ICommandRegistry _registry;
	    private readonly ICompositeApp _composite;

	    public CommandRegistryController(ICommandRegistry registry, ICompositeApp composite)
		{
			_registry = registry;
		    _composite = composite;

		    ViewBag.CompositeName = _composite.Name;
            ViewBag.CompositeDescription = _composite.Description;
		}

		public ActionResult Details(Guid publicationId)
		{
			var record = _registry.GetPublicationRecord(publicationId);

			return
				View(
					new PublishedCommandModel { HasValue = record != null, Record = record, PublicationId = publicationId.ToString() });
		}

		public ActionResult Index(int pageSize = 100, int offset = 0)
		{
			var records = _registry.GetRecords(pageSize, offset);

			return View(new PublishedCommandsModel { CurrentPage = offset, RowsPerPage = pageSize, Records = records });
		}
	}
}