using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Euclid.Framework.Cqrs.Metadata;
using Euclid.SDK.TestingFakes.Composites.InputModel;

namespace Euclid.Composites.Mvc.Controllers
{
	public class CommandController : Controller
	{
		public ViewResult Inspect(ICommandMetadata metadata)
		{
			// JT: find input map for metadata.Type

			if (metadata == null)
			{
				ViewBag.CommandName = "No command specified";
			}
			else
			{
				ViewBag.CommandName = "Found metadata for: " + metadata.Name;
			}

			return View(new FakeInputModel());
		}

		public ViewResult List(IList<ICommandMetadata> commands)
		{
			throw new NotImplementedException();
		}
	}
}