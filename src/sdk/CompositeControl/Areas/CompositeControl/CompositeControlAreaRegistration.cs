using System.Web.Mvc;

namespace CompositeControl.Areas.CompositeControl
{
	public class CompositeControlAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "compositecontrol";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
				context.MapRoute(
					"CompositeControl-Home", "compositecontrol/{action}", new { controller = "Composite", action = "Index" });

				context.MapRoute(
					"CompositeControl-AllAgentsWithFormat",
					"compositecontrol/agents/index.{format}",
					new { controller = "Agents", action = "Index" });

				context.MapRoute(
					"CompositeControl-AllAgents", "compositecontrol/agents", new { controller = "Agents", action = "Index" });

				context.MapRoute(
					"CompositeControl-Agent",
					"compositecontrol/agents/{agentSystemName}",
					new { controller = "Agents", action = "ViewAgent" });

				context.MapRoute(
					"CompositeControl-AgentWithFormat",
					"compositecontrol/agents/{agentSystemName}.{format}",
					new { controller = "Agents", action = "ViewAgent" });

				context.MapRoute(
					"CompositeControl-AgentPartsWithFormat",
					"compositecontrol/agents/{agentSystemName}/{descriptiveName}.{format}",
					new { controller = "Agents", action = "ViewPartCollection" });

				context.MapRoute(
					"CompositeControl-AgentParts",
					"compositecontrol/agents/{agentSystemName}/{descriptiveName}",
					new { controller = "Agents", action = "ViewPartCollection" });

				context.MapRoute(
					"CompositeControl-AgentPartWithFormat",
					"compositecontrol/agents/{agentSystemName}/{action}/{partName}.{format}",
					new { controller = "Agents", action = "ViewPart" });

				context.MapRoute(
					"CompositeControl-AgentPart",
					"compositecontrol/agents/{agentSystemName}/{action}/{partName}",
					new { controller = "Agents", action = "ViewPart" });

				context.MapRoute(
					"CompositeControl-CommandRegistryIndex",
					"compositecontrol/commandregistry",
					new { controller = "CommandRegistry", action = "Index" });

				context.MapRoute(
					"CompositeControl-CommandRegistryDetails",
					"compositecontrol/commandregistry/{publicationId}",
					new { controller = "CommandRegistry", action = "Details" });
		}
	}
}