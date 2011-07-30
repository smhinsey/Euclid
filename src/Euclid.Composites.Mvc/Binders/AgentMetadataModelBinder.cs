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
            var systemName = controllerContext.GetAgentSystemName();

            return _resolvers.GetAgentInfo(systemName);
        }

        public bool IsMatch(Type modelType)
        {
            return (modelType == typeof (IAgentInfo));
        }
    }
}