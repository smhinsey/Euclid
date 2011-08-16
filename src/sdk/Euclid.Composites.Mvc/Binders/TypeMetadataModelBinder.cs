using System;
using System.Linq;
using System.Web.Mvc;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Extensions;
using Euclid.Composites.Mvc.ActionFilters;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Agent.Metadata;

namespace Euclid.Composites.Mvc.Binders
{
	public class TypeMetadataModelBinder : IEuclidModelBinder
	{
		private readonly IAgentResolver[] _resolvers;

		public TypeMetadataModelBinder(IAgentResolver[] resolvers)
		{
			_resolvers = resolvers;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var systemName = controllerContext.GetAgentSystemName();

			var partName = controllerContext.GetPartName();

			var metadata = _resolvers.GetAgentMetadata(systemName);

			var partMetadata = metadata.ReadModels.Where(x => x.Name == partName).FirstOrDefault();

			if (partMetadata == null)
			{
				partMetadata = metadata.Queries.Where(x => x.Name == partName).FirstOrDefault();
			}

            if (partMetadata == null)
            {
                partMetadata = metadata.Commands.Where(x => x.Name == partName).FirstOrDefault();
            }

            if (partMetadata == null)
            {
                throw new AgentPartMetdataNotFoundException();
            }

			return partMetadata;
		}

		public bool IsMatch(Type modelType)
		{
			return typeof (ITypeMetadata).IsAssignableFrom(modelType);
		}
	}
}