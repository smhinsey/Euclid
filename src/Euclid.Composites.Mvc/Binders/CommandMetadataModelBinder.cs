using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Euclid.Composites.Agent;
using Euclid.Composites.Extensions;
using Euclid.Framework.Metadata;

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
            var agentSystemName = controllerContext.GetAgentSystemName();

            var commandName = controllerContext.GetCommandName();

            var action = controllerContext.GetAction();

            var agentInfo = _resolvers.GetAgentInfo(agentSystemName);

            if (action == "list")
            {
                return agentInfo.Commands;
            }
            else if (action == "inspect")
            {
                return agentInfo.GetCommandMetadata(commandName);
            }

            return null;
        }

        public bool IsMatch(Type modelType)
        {
            return modelType == typeof(IEuclidMetdata) ||
                   modelType == typeof(IEnumerable<IEuclidMetdata>);
        }
    }
}