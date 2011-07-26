using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Euclid.Agent;
using Euclid.Composites.Mvc.Binders;
using Euclid.Composites.Mvc.ComponentRegistration;

namespace Euclid.Composites.Mvc
{
	public class EuclidComposite
	{
		private static readonly IWindsorContainer Container = new WindsorContainer();

		public void Configure()
		{
			Container.Install(new ContainerInstaller());

			Container.Install(new ControllerContainerInstaller());

			RegisterModelBinders();

			ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(Container));

			RegisterControllers();
		}

		// called by agent package as it's installed via nuget
		public static void InstallAgent(Assembly agent)
		{
			//retrieve metadata defined in AgentInfo.cs

			var attributes = agent.GetCustomAttributes(typeof (AgentNameAttribute), false);

			if (attributes.Length > 0)
			{
				var agentName = attributes[0];
			}
		}

		private static void RegisterControllers()
		{
			RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			RouteTable.Routes.MapRoute(
			                           "Agent",
			                           "{controller}/{action}/{scheme}+{systemName}",
			                           new {controller = "Agent", action = "Operations"});

			RouteTable.Routes.MapRoute(
			                           "Command",
			                           "{controller}/{action}/{scheme}+{systemName}/{command}",
			                           new {controller = "Command", action = "Inspect", command = UrlParameter.Optional}
				);

			// JT: add route for query controller when it exists
		}

		private static void RegisterModelBinders()
		{
			Container.Install(new ModelBinderIntstaller());

			ModelBinders.Binders.DefaultBinder = new EuclidDefaultBinder(Container.ResolveAll<IEuclidModelBinder>());
		}
	}
}