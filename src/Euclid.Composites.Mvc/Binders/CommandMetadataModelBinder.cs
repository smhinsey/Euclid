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
            var agentSystemName = controllerContext.GetAgentSystemName();

            var commandName = controllerContext.GetCommandName();

            var action = controllerContext.GetAction();

            var agentInfo = _resolvers.GetAgentInfo(agentSystemName);

            if (action == "list")
            {
                return agentInfo.GetCommands();
            }
            else if (action == "inspect")
            {
                return agentInfo.GetCommand(commandName);
            }

            return null;
        }

        public bool IsMatch(Type modelType)
        {
            return modelType == typeof (ICommandMetadata) ||
                    modelType == typeof(IEnumerable<ICommandMetadata>);
        }
    }
}