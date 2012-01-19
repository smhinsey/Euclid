using System;
using System.Linq;
using Euclid.Composites;
using Euclid.Composites.Mvc.ActionFilters;
using JsonCompositeInspector.Models;
using Nancy;

namespace JsonCompositeInspector.Module
{
	public class CompositeModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;

		public CompositeModule(ICompositeApp compositeApp)
			: base("composite")
		{
			_compositeApp = compositeApp;

			Get[""] = _ => { return View["Shared/Frameset.cshtml"]; };

			Get["/home"] = _ => {
			               		var model = new CompositeHome
			               		            	{
			               		            		Agents = _compositeApp.Agents,
			               		            		ConfigurationErrors = _compositeApp.IsValid() ? null : _compositeApp.GetConfigurationErrors(),
			               		            		IsValid = _compositeApp.IsValid(),
			               		            		CompositeDescription = _compositeApp.Description,
			               		            		CompositeName = _compositeApp.Name
			               		            	};
			               		return View["Composite/home.cshtml", model];
			               	};

			Get["/agent/{agentSystemName}"] = p =>
			                                  	{
			                                  		var agent = compositeApp
																.Agents
																.Where(
																	a => a.SystemName.Equals(p.agentSystemName, StringComparison.InvariantCultureIgnoreCase)).
			                                  					FirstOrDefault();

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

			                                  		return View["Composite/view-agent.cshtml", model];
			                                  	};
		}
	}
}