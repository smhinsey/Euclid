using System;
using System.Web.Mvc;
using CompositeInspector.Models;
using Euclid.Composites;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Cqrs;
using MvcContrib.Filters;

namespace CompositeInspector.Controllers
{
	[Layout("_Layout")]
	public class CommandRegistryController : Controller
	{
		private readonly ICompositeApp _composite;

		private readonly ICommandRegistry _registry;

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
					new PublishedCommandModel { Record = record, PublicationId = publicationId.ToString() });
		}

		public ActionResult Index(int pageSize = 100, int offset = 0)
		{
			var records = _registry.GetRecords(pageSize, offset);

			records.SetupPaging(this, pageSize, offset);

			return View(records);
		}
	}
}