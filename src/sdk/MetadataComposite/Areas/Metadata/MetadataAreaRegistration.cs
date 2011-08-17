using System.Web.Mvc;

namespace MetadataComposite.Areas.Metadata
{
    public class MetadataAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Metadata";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("AllAgentsWithFormat", "agents/index.{format}",
                            new { controller = "Agents", action = "Index" });

            context.MapRoute("AllAgents", "agents",
                            new { controller = "Agents", action = "Index" });

            context.MapRoute("Agent", "agents/{agentSystemName}",
                            new { controller = "Agents", action = "ViewAgent" });

            context.MapRoute("AgentWithFormat", "agents/{agentSystemName}.{format}",
                            new { controller = "Agents", action = "ViewAgent" });

            context.MapRoute("AgentPartsWithFormat", "agents/{agentSystemName}/{descriptiveName}.{format}",
                            new { controller = "Agents", action = "ViewPartCollection" });

            context.MapRoute("AgentParts", "agents/{agentSystemName}/{descriptiveName}",
                            new { controller = "Agents", action = "ViewPartCollection" });

            context.MapRoute("AgentPartWithFormat", "agents/{agentSystemName}/{action}/{partName}.{format}",
                            new { controller = "Agents", action = "ViewPart" });

            context.MapRoute("AgentPart", "agents/{agentSystemName}/{action}/{partName}",
                            new { controller = "Agents", action = "ViewPart" });
        }
    }
}
