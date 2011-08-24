using System.Web.Mvc;

namespace AgentPanel.Areas.Metadata
{
	public class MetadataAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return "Metadata"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.Routes.Clear();

            context.MapRoute("Composite", "metadata/composite/{action}",
                 new { controller = "Composite", action = "Index" });

			context.MapRoute("AllAgentsWithFormat", "metadata/agents/index.{format}",
			                 new {controller = "Agents", action = "Index"});

			context.MapRoute("AllAgents", "metadata/agents",
			                 new {controller = "Agents", action = "Index"});

			context.MapRoute("Agent", "metadata/agents/{agentSystemName}",
			                 new {controller = "Agents", action = "ViewAgent"});

			context.MapRoute("AgentWithFormat", "metadata/agents/{agentSystemName}.{format}",
			                 new {controller = "Agents", action = "ViewAgent"});

			context.MapRoute("AgentPartsWithFormat", "metadata/agents/{agentSystemName}/{descriptiveName}.{format}",
			                 new {controller = "Agents", action = "ViewPartCollection"});

			context.MapRoute("AgentParts", "metadata/agents/{agentSystemName}/{descriptiveName}",
			                 new {controller = "Agents", action = "ViewPartCollection"});

			context.MapRoute("AgentPartWithFormat", "metadata/agents/{agentSystemName}/{action}/{partName}.{format}",
			                 new {controller = "Agents", action = "ViewPart"});

			context.MapRoute("AgentPart", "metadata/agents/{agentSystemName}/{action}/{partName}",
			                 new {controller = "Agents", action = "ViewPart"});
		}
	}
}