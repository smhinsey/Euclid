using System;
using System.Web.Mvc;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Extensions;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composites.Mvc.Binders
{
	public class TypeMetadataBinder : IEuclidModelBinder
	{
		private readonly IAgentResolver[] _resolvers;

		public TypeMetadataBinder(IAgentResolver[] resolvers)
		{
			_resolvers = resolvers;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var systemName = controllerContext.GetAgentSystemName();

			var partName = controllerContext.GetPartName();

			var metadata = _resolvers.GetAgentMetadata(systemName);

			var typeMetadata = metadata.GetPartByTypeName(partName);

			if (typeMetadata == null)
			{
				throw new TypeMetadataNotFoundException();
			}

			return typeMetadata;
		}

		public bool IsMatch(Type modelType)
		{
			return typeof (ITypeMetadata).IsAssignableFrom(modelType);
		}
	}
}