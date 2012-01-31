using System;
using CompositeInspector.Models;
using Euclid.Composites;
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

		private readonly ICompositeApp _compositeApp;

		public UserInterfaceModule(ICompositeApp compositeApp)
			: base(BaseRoute)
		{
			_compositeApp = compositeApp;

			Get[IndexRoute] = _ => View[IndexViewPath];

			Get[TemplateRoute] = p => View[String.Concat(TemplateViewFolder, (string)p.templateName, ViewExtension)];

			Get[AgentDetailRoute] = p => View[AgentDetailView];

			Get[CompositeDetailRoute] = _ => View[CompositeDetailView];

			Get[HomeRoute] = _ =>
				{
					var model = new CompositeHome
						{
							Agents = _compositeApp.Agents,
							ConfigurationErrors = _compositeApp.IsValid() ? null : _compositeApp.GetConfigurationErrors(),
							IsValid = _compositeApp.IsValid(),
							CompositeDescription = _compositeApp.Description,
							CompositeName = _compositeApp.Name
						};
					return View[HomeViewPath, model];
				};
		}
	}
}