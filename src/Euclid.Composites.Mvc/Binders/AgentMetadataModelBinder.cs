using System;
using System.Linq;
using System.Web.Mvc;
using Euclid.Agent;
using Euclid.Composites.Agent;
using Euclid.Composites.Extensions;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Mvc.Binders
{
	public class AgentMetadataModelBinder : IEuclidModelBinder
	{
		private readonly IAgentResolutionStrategy[] _resolvers;

		public AgentMetadataModelBinder(IAgentResolutionStrategy[] resolvers)
		{
			_resolvers = resolvers;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var scheme = (string) controllerContext.RouteData.Values["scheme"];

			var systemName = (string) controllerContext.RouteData.Values["systemName"];

			if (string.IsNullOrEmpty(scheme) || string.IsNullOrEmpty(systemName)) return null;

			var agent = _resolvers.Select(rslvr => rslvr.GetAgent(scheme, systemName)).FirstOrDefault(assembly => assembly != null);

			if (agent == null)
			{
				throw new AgentNotFoundException(scheme, systemName);
			}

			return agent.GetAgentMetadata();
		}

		public bool IsMatch(Type modelType)
		{
			return (modelType == typeof (IAgentMetadata));
		}
	}
}