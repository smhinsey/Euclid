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
		private const string HomeRoute = "/home";
		private const string HomeViewPath = "Shared/Frameset.cshtml";

		private const string AgentDetailRoute = "/agent/{agentSystemName}";
		private const string AgentDetailView = "view-agent.cshtml";
		
		private const string CompositeDetailRoute = "/details";
		private const string CompositeDetailView = "view-composite.cshtml";
			
		private const string CommandRegistryRoute = "/command-registry";
		private const string CommandRegistryView = "view-command-registry";

		public UserInterfaceModule() : base(BaseRoute)
		{
			Get[IndexRoute] = _ => Response.AsRedirect(CompositeDetailView, RedirectResponse.RedirectType.Temporary); //{ throw new NotImplementedException("<a href='/composite/details'>details</a>"); };

			Get[AgentDetailRoute] = p => View[AgentDetailView];

			Get[CompositeDetailRoute] = _ => View[CompositeDetailView];

			Get[CommandRegistryRoute] = _ => View[CommandRegistryView];

			Get[HomeRoute] = _ => Response.AsRedirect(CompositeDetailView, RedirectResponse.RedirectType.Temporary);
		}
	}
}