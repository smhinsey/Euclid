using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace CompositeInspector
{
	public class CompositeInspectorAreaRegistration : PortableAreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "CompositeInspector";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
		{
			/********************************
            * Routes for HTML layout assets
            *   images, styles, & scripts must exist directly below the listed directories
            *   or the MvcContrib.EmbeddedResourceController won't find them
            ********************************/

			context.MapRoute(
				"CompositeInspector-Images",
				"CompositeInspector/Images/{resourceName}",
				new { controller = "EmbeddedResource", action = "Index", resourcePath = "Content.Images" },
				new[] { "MvcContrib.PortableAreas" });

			context.MapRoute(
				"CompositeInspector-Css-Images",
				"CompositeInspector/Styles/Images/{resourceName}",
				new { controller = "EmbeddedResource", action = "Index", resourcePath = "Content.Images" },
				new[] { "MvcContrib.PortableAreas" });

			context.MapRoute(
				"CompositeInspector-Styles",
				"CompositeInspector/Styles/{resourceName}",
				new { controller = "EmbeddedResource", action = "Index", resourcePath = "Content.Styles" },
				new[] { "MvcContrib.PortableAreas" });

			context.MapRoute(
				"CompositeInspector-Scripts",
				"CompositeInspector/Scripts/{resourceName}",
				new { controller = "EmbeddedResource", action = "Index", resourcePath = "Content.Scripts" },
				new[] { "MvcContrib.PortableAreas" });

			/********************************
            * Routes for controllers
            ********************************/
			context.MapRoute(
				"CompositeInspector-AllAgentsWithFormat",
				"CompositeInspector/agents/index.{format}",
				new { controller = "Agents", action = "Index" });

			context.MapRoute(
				"CompositeInspector-PublishCommand",
				"CompositeInspector/agents/publish",
				new { controller = "Agents", action = "Publish" });

			context.MapRoute(
				"CompositeInspector-PublishCommandWithView",
				"CompositeInspector/agents/publishandviewdetails",
				new { controller = "Agents", action = "PublishAndViewDetails" });

			context.MapRoute(
				"CompositeInspector-Agent",
				"CompositeInspector/agents/{agentSystemName}",
				new { controller = "Agents", action = "ViewAgent" });

			context.MapRoute(
				"CompositeInspector-AgentWithFormat",
				"CompositeInspector/agents/{agentSystemName}.{format}",
				new { controller = "Agents", action = "ViewAgent" });

			context.MapRoute(
				"CompositeInspector-AgentPartsWithFormat",
				"CompositeInspector/agents/{agentSystemName}/{descriptiveName}.{format}",
				new { controller = "Agents", action = "ViewPartCollection" });

			context.MapRoute(
				"CompositeInspector-AgentParts",
				"CompositeInspector/agents/{agentSystemName}/{descriptiveName}",
				new { controller = "Agents", action = "ViewPartCollection" });

			context.MapRoute(
				"CompositeInspector-AgentPartWithFormat",
				"CompositeInspector/agents/{agentSystemName}/{action}/{partName}.{format}",
				new { controller = "Agents", action = "ViewPart" });

			context.MapRoute(
				"CompositeInspector-AgentPart",
				"CompositeInspector/agents/{agentSystemName}/{action}/{partName}",
				new { controller = "Agents", action = "ViewPart" });

			context.MapRoute(
				"CompositeInspector-CommandRegistryDetails",
				"CompositeInspector/commandregistry/{publicationId}",
				new { controller = "CommandRegistry", action = "Details" });

			context.MapRoute(
				"CompositeInspector-default",
				"CompositeInspector/{controller}/{action}",
				new { controller = "Frame", action = "Index" });

			RegisterAreaEmbeddedResources();
		}
	}
}