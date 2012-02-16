using System;
using Euclid.Composites;
using LoggingAgent.Queries;
using Nancy;

namespace CompositeInspector.Module
{
	public class UserInterfaceModule : NancyModule
	{
		private const string BaseRoute = "composite";
		private const string IndexRoute = "";
		private const string TemplateViewFolder = "Templates/";
		private const string ViewExtension = ".cshtml";
		private const string AgentDetailRoute = "/agent/{agentSystemName}";
		private const string AgentDetailView = "view-agent.cshtml";
		private const string CompositeDetailRoute = "/details";
		private const string CompositeDetailView = "view-composite.cshtml";
		private const string TemplateRoute = "/ui/template/{templateName}";
		private const string HomeRoute = "/home";
		private const string HomeViewPath = "Composite/home.cshtml";
		private const string IndexViewPath = "Shared/Frameset.cshtml";
		private const string CommandRegistryRoute = "/command-registry";
		private const string CommandRegistryView = "view-command-registry";
		private readonly ICompositeApp _compositeApp;
		private readonly CommandRegistryQueries _registryQueries;

		public UserInterfaceModule(ICompositeApp compositeApp, CommandRegistryQueries registryQueries)
			: base(BaseRoute)
		{
			_compositeApp = compositeApp;
			_registryQueries = registryQueries;

			Get[IndexRoute] = _ => View[IndexViewPath];

			Get[TemplateRoute] = p => View[String.Concat(TemplateViewFolder, (string)p.templateName, ViewExtension)];

			Get[AgentDetailRoute] = p => View[AgentDetailView];

			Get[CompositeDetailRoute] = _ => View[CompositeDetailView];

			Get[HomeRoute] = _ =>
			                 	{
			                 		throw new NotImplementedException();
			                 		//var model = new CompositeHome
			                 		//    {
			                 		//        Agents = _compositeApp.Agents,
			                 		//        ConfigurationErrors = _compositeApp.IsValid() ? null : _compositeApp.GetConfigurationErrors(),
			                 		//        IsValid = _compositeApp.IsValid(),
			                 		//        CompositeDescription = _compositeApp.Description,
			                 		//        CompositeName = _compositeApp.Name
			                 		//    };
			                 		//return View[HomeViewPath, model];
			                 	};

			Get[CommandRegistryRoute] = _ => View[CommandRegistryView];
		}

	}
}