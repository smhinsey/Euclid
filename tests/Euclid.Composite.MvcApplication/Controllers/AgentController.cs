﻿using System.Web.Mvc;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composite.MvcApplication.Controllers
{
	public class AgentController : Controller
	{
		public ViewResult Operations(IAgentMetadata metadata)
		{
			return View(metadata);
		}
	}
}