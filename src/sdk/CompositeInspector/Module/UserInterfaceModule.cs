using System;
using Euclid.Composites;
using LoggingAgent.Queries;
using Nancy;
using Nancy.Responses;

namespace CompositeInspector.Module
{
	public class UserInterfaceModule : NancyModule
	{
		private const string BaseRoute = "composite";

		private const string IndexRoute = "";
		private const string HomeRoute = "/inspector";
		private const string HomeViewPath = "inspector";

		private const string AgentDetailRoute = "/agent/{agentSystemName}";
		private const string AgentDetailView = "view-agent.cshtml";
		
		private const string CompositeDetailRoute = "/details";

		private const string CommandRegistryRoute = "/command-registry";
		private const string CommandRegistryView = "view-command-registry";

		private const string LogsRoute = "/Logs";
		private const string LogsView = "view-logs";
		
		public UserInterfaceModule()
			: base(BaseRoute)
		{
			Get[IndexRoute] = _ => View[HomeViewPath];

			Get[HomeRoute] = _ => View[HomeViewPath];

			Get[AgentDetailRoute] = p => View[AgentDetailView];

			Get[CommandRegistryRoute] = _ => View[CommandRegistryView];

			Get[CompositeDetailRoute] = _ => Response.AsRedirect("/composite/", RedirectResponse.RedirectType.Permanent);
		}
	}
}