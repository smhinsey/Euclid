using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace CompositeInspector
{
    public class CompositeControlAreaRegistration : PortableAreaRegistration
    {
        public override string AreaName
        {
            get { return "CompositeInspector"; }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {
            /********************************
            * Routes for HTML layout assets
            *   images, styles, & scripts must exist directly below the listed directories
            *   or the MvcContrib.EmbeddedResourceController won't find them
            ********************************/

            context.MapRoute("images",
                             "CompositeInspector/Images/{resourceName}",
                             new {controller = "EmbeddedResource", action = "Index", resourcePath = "Content.Images"},
                             new[] {"MvcContrib.PortableAreas"}
                );

            context.MapRoute("styles",
                             "CompositeInspector/Styles/{resourceName}",
                             new { controller = "EmbeddedResource", action = "Index", resourcePath = "Content.Styles" },
                             new[] { "MvcContrib.PortableAreas" }
                );

            context.MapRoute("scripts",
                 "CompositeInspector/Scripts/{resourceName}",
                 new { controller = "EmbeddedResource", action = "Index", resourcePath = "Content.Scripts" },
                             new[] { "MvcContrib.PortableAreas" }
                );

            /********************************
            * Routes for controllers
            ********************************/
            context.MapRoute(
                "CompositeControl-AllAgentsWithFormat",
                "CompositeInspector/agents/index.{format}",
                new {controller = "Agents", action = "Index"});

            context.MapRoute(
                "CompositeControl-Agent",
                "CompositeInspector/agents/{agentSystemName}",
                new {controller = "Agents", action = "ViewAgent"});

            context.MapRoute(
                "CompositeControl-AgentWithFormat",
                "CompositeInspector/agents/{agentSystemName}.{format}",
                new {controller = "Agents", action = "ViewAgent"});

            context.MapRoute(
                "CompositeControl-AgentPartsWithFormat",
                "CompositeInspector/agents/{agentSystemName}/{descriptiveName}.{format}",
                new {controller = "Agents", action = "ViewPartCollection"});

            context.MapRoute(
                "CompositeControl-AgentParts",
                "CompositeInspector/agents/{agentSystemName}/{descriptiveName}",
                new {controller = "Agents", action = "ViewPartCollection"});

            context.MapRoute(
                "CompositeControl-AgentPartWithFormat",
                "CompositeInspector/agents/{agentSystemName}/{action}/{partName}.{format}",
                new {controller = "Agents", action = "ViewPart"});

            context.MapRoute(
                "CompositeControl-AgentPart",
                "CompositeInspector/agents/{agentSystemName}/{action}/{partName}",
                new {controller = "Agents", action = "ViewPart"});

            context.MapRoute(
                "CompositeControl-CommandRegistryDetails",
                "CompositeInspector/commandregistry/{publicationId}",
                new {controller = "CommandRegistry", action = "Details"});

            context.MapRoute(
                "CompositeControl-default",
                "CompositeInspector/{controller}/{action}",
                new { controller = "Composite", action = "Index" });

            RegisterAreaEmbeddedResources();
        }
    }
}