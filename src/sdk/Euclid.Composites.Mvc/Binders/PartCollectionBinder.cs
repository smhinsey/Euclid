using System;
using System.Web.Mvc;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Extensions;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.AgentMetadata;

namespace Euclid.Composites.Mvc.Binders
{
	public class PartCollectionBinder : IEuclidModelBinder
	{
		private readonly IAgentResolver[] _resolvers;

		public PartCollectionBinder(IAgentResolver[] resolvers)
		{
			_resolvers = resolvers;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var systemName = controllerContext.GetAgentSystemName();

			var metadata = _resolvers.GetAgentMetadata(systemName);

			var partName = controllerContext.GetPartName();

			var descriptiveName = controllerContext.GetRouteValue<string>("descriptiveName");

			IPartCollection partCollection;
			if (string.IsNullOrEmpty(partName))
			{
				partCollection = metadata.GetPartCollectionByDescriptiveName(descriptiveName);
			}
			else
			{
				partCollection = metadata.GetPartCollectionContainingPartName(partName);
			}

			if (partCollection == null)
			{
				throw new PartCollectionNotFoundException();
			}

			return partCollection;
		}

		public bool IsMatch(Type modelType)
		{
			return typeof(IPartCollection).IsAssignableFrom(modelType);
		}
	}
}