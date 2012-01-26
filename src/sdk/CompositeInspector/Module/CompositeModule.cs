using System;
using System.Linq;
using CompositeInspector.Models;
using Euclid.Composites;
using Euclid.Composites.Mvc.ActionFilters;
using Nancy;

namespace CompositeInspector.Module
{
	public class CompositeModule : NancyModule
	{
		private const string AgentDetailRoute = "/agent/{agentSystemName}";

		private const string AgentDetailViewPath = "Composite/view-agent.cshtml";

		private const string BaseRoute = "composite";

		private const string HomeRoute = "/home";

		private const string HomeViewPath = "Composite/home.cshtml";

		private const string IndexRoute = "";

		private const string IndexViewPath = "Shared/Frameset.cshtml";

		private readonly ICompositeApp _compositeApp;

		public CompositeModule(ICompositeApp compositeApp)
			: base(BaseRoute)
		{
			_compositeApp = compositeApp;

			Get[IndexRoute] = _ => View[IndexViewPath];

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

			Get[AgentDetailRoute] = p =>
				{
					var agents = compositeApp.Agents;

					var agent =
						agents.FirstOrDefault(a => a.SystemName.Equals(p.agentSystemName, StringComparison.InvariantCultureIgnoreCase));

					if (agent == null)
					{
						throw new AgentMetadataNotFoundException();
					}

					var model = new AgentModel
						{
							AgentSystemName = p.agentSystemName,
							Commands = agent.Commands,
							Description = agent.Description,
							DescriptiveName = agent.DescriptiveName,
							Queries = agent.Queries,
							ReadModels = agent.ReadModels
						};

					return View[AgentDetailViewPath, model];
				};
		}
	}
}