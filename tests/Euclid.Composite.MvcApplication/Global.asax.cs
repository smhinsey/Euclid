using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Composite.MvcApplication.EuclidConfiguration.TypeConverters;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using Euclid.Sdk.FakeAgent.Commands;

namespace Euclid.Composite.MvcApplication
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);

			RegisterRoutes(RouteTable.Routes);

			var container = new WindsorContainer();

			var composite = new MvcCompositeApp(container);

			var euclidCompositeConfiguration = new CompositeAppSettings();

			/*
             * jt: this is how a composite developer would override the default settings for the mvc composite
             * euclidCompositeConfiguration.BlobStorage.ApplyOverride(typeof(SomeBlobStorageImplementation));
             */

			composite.Configure(euclidCompositeConfiguration);

			composite.AddAgent(typeof (FakeCommand).Assembly);

			composite.RegisterInputModel(new InputToFakeCommand4Converter());

			container.Register(Component.For<ICompositeApp>().Instance(composite));

			Error += composite.LogUnhandledException;

			BeginRequest += composite.BeginPageRequest;
		}

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("AllAgentsWithFormat", "agents/index.{format}",
			                new {controller = "Agents", action = "Index"});

			routes.MapRoute("AllAgents", "agents",
			                new {controller = "Agents", action = "Index"});

			routes.MapRoute("Agent", "agents/{agentSystemName}",
			                new {controller = "Agents", action = "ViewAgent"});

			routes.MapRoute("AgentWithFormat", "agents/{agentSystemName}.{format}",
			                new {controller = "Agents", action = "ViewAgent"});

			routes.MapRoute("AgentPartsWithFormat", "agents/{agentSystemName}/{descriptiveName}.{format}",
			                new {controller = "Agents", action = "ViewPartCollection"});

            routes.MapRoute("AgentParts", "agents/{agentSystemName}/{descriptiveName}",
                            new { controller = "Agents", action = "ViewPartCollection" });

			routes.MapRoute("AgentPartWithFormat", "agents/{agentSystemName}/{action}/{partName}.{format}",
			                new {controller = "Agents", action = "ViewPart"});

			routes.MapRoute("AgentPart", "agents/{agentSystemName}/{action}/{partName}",
			                new {controller = "Agents", action = "ViewPart"});
		}
	}
}