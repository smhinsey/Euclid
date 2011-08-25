using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace PortableAgentPanel
{
	public class CompositeControlAreaRegistration : PortableAreaRegistration
	{
		public override string AreaName
		{
			get
			{
                return "PortableAgentPanel";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
		{
            context.MapRoute("resources",
               AreaName + "/Resource/{resourceName}",
               new { Controller = "EmbeddedResource", action = "Index" },
               new string[] { "MvcContrib.PortableAreas" });

				context.MapRoute(
                    "CompositeControl-Home", 
                    "PortableAgentPanel/{action}", 
                    new { controller = "Composite", action = "Index" });

                context.MapRoute(
                    "CompositeControl-AllAgents",
                    "PortableAgentPanel/agents",
                    new { controller = "Agents", action = "Index" });

                context.MapRoute(
					"CompositeControl-AllAgentsWithFormat",
                    "PortableAgentPanel/agents/index.{format}",
					new { controller = "Agents", action = "Index" });

				context.MapRoute(
					"CompositeControl-Agent",
                    "PortableAgentPanel/agents/{agentSystemName}",
					new { controller = "Agents", action = "ViewAgent" });

				context.MapRoute(
					"CompositeControl-AgentWithFormat",
                    "PortableAgentPanel/agents/{agentSystemName}.{format}",
					new { controller = "Agents", action = "ViewAgent" });

				context.MapRoute(
					"CompositeControl-AgentPartsWithFormat",
                    "PortableAgentPanel/agents/{agentSystemName}/{descriptiveName}.{format}",
					new { controller = "Agents", action = "ViewPartCollection" });

				context.MapRoute(
					"CompositeControl-AgentParts",
                    "PortableAgentPanel/agents/{agentSystemName}/{descriptiveName}",
					new { controller = "Agents", action = "ViewPartCollection" });

				context.MapRoute(
					"CompositeControl-AgentPartWithFormat",
                    "PortableAgentPanel/agents/{agentSystemName}/{action}/{partName}.{format}",
					new { controller = "Agents", action = "ViewPart" });

				context.MapRoute(
					"CompositeControl-AgentPart",
                    "PortableAgentPanel/agents/{agentSystemName}/{action}/{partName}",
					new { controller = "Agents", action = "ViewPart" });

				context.MapRoute(
					"CompositeControl-CommandRegistryIndex",
                    "PortableAgentPanel/commandregistry",
					new { controller = "CommandRegistry", action = "Index" });

				context.MapRoute(
					"CompositeControl-CommandRegistryDetails",
                    "PortableAgentPanel/commandregistry/{publicationId}",
					new { controller = "CommandRegistry", action = "Details" });

		    RegisterAreaEmbeddedResources();
		}
	}
}