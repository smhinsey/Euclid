using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Euclid.Composite.MvcApplication.EuclidConfiguration.TypeConverters;
using Euclid.Composites.Mvc;
using Euclid.Framework.TestingFakes.Cqrs;

namespace Euclid.Composite.MvcApplication
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class AgentViewerComposite : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);

			RegisterRoutes(RouteTable.Routes);

			var composite = new MvcCompositeApp();

			var euclidCompositeConfiguration = new MvcCompositeAppSettings();

			/*
             * jt: this is how a composite developer would override the default settings for the mvc composite
             * euclidCompositeConfiguration.BlobStorage.ApplyOverride(typeof(SomeBlobStorageImplementation));
             */

			composite.Configure(this, euclidCompositeConfiguration);

			composite.InstallAgent(typeof (FakeCommand4).Assembly);

			composite.RegisterInputModel(new InputToFakeCommand4Converter());
		}

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
			                "Command",
			                "{controller}/{action}/{agentSystemName}/{commandName}",
			                new {controller = "Command", action = "Inspect", commandName = UrlParameter.Optional}
				);

			routes.MapRoute(
			                "InspectCommand",
			                "{agentSystemName}/{commandName}.{format}",
			                new {controller = "Command", action = "Inspect", format = UrlParameter.Optional}
				);
		}
	}
}