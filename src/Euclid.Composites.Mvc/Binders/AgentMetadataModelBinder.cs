using System;
using System.Web.Mvc;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Extensions;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Mvc.Binders
{
	public class AgentMetadataModelBinder : IEuclidModelBinder
	{
		private readonly IAgentResolver[] _resolvers;

		public AgentMetadataModelBinder(IAgentResolver[] resolvers)
		{
			_resolvers = resolvers;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var systemName = controllerContext.GetAgentSystemName();

			return _resolvers.GetAgentMetadata(systemName);
		}

		public bool IsMatch(Type modelType)
		{
			return (modelType == typeof (IAgentMetadata));
		}
	}
}