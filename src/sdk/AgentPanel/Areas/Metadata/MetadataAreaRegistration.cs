using System.Web.Mvc;

namespace AgentPanel.Areas.Metadata
{
	public class MetadataAreaRegistration : AreaRegistration
	{
		private static bool _routesAlreadyAdded;

		public override string AreaName
		{
			get { return "metadata"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			if (!_routesAlreadyAdded)
			{

				context.MapRoute(
					"AgentPanel-Composite", "metadata/composite/{action}", new { controller = "Composite", action = "Index" });

				context.MapRoute(
					"AgentPanel-AllAgentsWithFormat", "metadata/agents/index.{format}", new { controller = "Agents", action = "Index" });

				context.MapRoute("AgentPanel-AllAgents", "metadata/agents", new { controller = "Agents", action = "Index" });

				context.MapRoute(
					"AgentPanel-Agent", "metadata/agents/{agentSystemName}", new { controller = "Agents", action = "ViewAgent" });

				context.MapRoute(
					"AgentPanel-AgentWithFormat",
					"metadata/agents/{agentSystemName}.{format}",
					new { controller = "Agents", action = "ViewAgent" });

				context.MapRoute(
					"AgentPanel-AgentPartsWithFormat",
					"metadata/agents/{agentSystemName}/{descriptiveName}.{format}",
					new { controller = "Agents", action = "ViewPartCollection" });

				context.MapRoute(
					"AgentPanel-AgentParts",
					"metadata/agents/{agentSystemName}/{descriptiveName}",
					new { controller = "Agents", action = "ViewPartCollection" });

				context.MapRoute(
					"AgentPanel-AgentPartWithFormat",
					"metadata/agents/{agentSystemName}/{action}/{partName}.{format}",
					new { controller = "Agents", action = "ViewPart" });

				context.MapRoute(
					"AgentPanel-AgentPart",
					"metadata/agents/{agentSystemName}/{action}/{partName}",
					new { controller = "Agents", action = "ViewPart" });

				context.MapRoute(
					"AgentPanel-CommandRegistry", "metadata/commandregistry", new { controller = "CommandRegistry", action = "Index" });

				_routesAlreadyAdded = true;
			}
		}
	}
}