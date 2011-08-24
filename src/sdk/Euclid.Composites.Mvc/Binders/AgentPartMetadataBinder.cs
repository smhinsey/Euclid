using System;
using System.Web.Mvc;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Extensions;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.AgentMetadata;

namespace Euclid.Composites.Mvc.Binders
{
	public class AgentPartMetadataBinder : IEuclidModelBinder
	{
		private readonly IAgentResolver[] _resolvers;

		public AgentPartMetadataBinder(IAgentResolver[] resolvers)
		{
			_resolvers = resolvers;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var systemName = controllerContext.GetAgentSystemName();

			var partName = controllerContext.GetPartName();

			var metadata = _resolvers.GetAgentMetadata(systemName);

			var agentPartMetadata = metadata.GetPartByTypeName(partName);

			if (agentPartMetadata == null)
			{
				throw new TypeMetadataNotFoundException();
			}

			return agentPartMetadata;
		}

		public bool IsMatch(Type modelType)
		{
			return typeof (IAgentPart).IsAssignableFrom(modelType);
		}
	}
}
