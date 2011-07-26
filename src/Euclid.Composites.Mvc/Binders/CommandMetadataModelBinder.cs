using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Euclid.Agent;
using Euclid.Composites.Agent;
using Euclid.Composites.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.Binders
{
	public class CommandMetadataModelBinder : IEuclidModelBinder
	{
		private readonly IAgentResolutionStrategy[] _resolvers;

		public CommandMetadataModelBinder(IAgentResolutionStrategy[] resolvers)
		{
			_resolvers = resolvers;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var scheme = (string) controllerContext.RouteData.Values["scheme"];

			var systemName = (string) controllerContext.RouteData.Values["systemName"];

			var command = (string) controllerContext.RouteData.Values["command"];

			var action = (string) controllerContext.RouteData.Values["action"];

			if (string.IsNullOrEmpty(scheme) || string.IsNullOrEmpty(systemName) || string.IsNullOrEmpty(action)) return null;

			var agent = _resolvers.Select(rslvr => rslvr.GetAgent(scheme, systemName)).FirstOrDefault(assembly => assembly != null);

			if (agent == null)
			{
				throw new AgentNotFoundException(scheme, systemName);
			}


			object model = null;

			switch (action.ToLower())
			{
				case "list":
					model = GetCommandList(agent);
					break;
				case "inspect":
					model = GetCommand(agent, command);
					break;
				default:
					return null;
			}

			return model;
		}

		public bool IsMatch(Type modelType)
		{
			return modelType == typeof (ICommandMetadata);
		}

		private ICommandMetadata GetCommand(Assembly agent, string commandName)
		{
			throw new NotImplementedException();
		}

		private IEnumerable<ICommandMetadata> GetCommandList(Assembly agent)
		{
			var agentMetadata = agent.GetAgentMetadata();

			var commandTypes = agent.GetTypes().Where(x => x.Namespace == agentMetadata.CommandNamespace && x.IsAssignableFrom(typeof (ICommand)));

			return null;
		}
	}
}