using System;
using System.Data;
using System.Web.Mvc;
using Euclid.Agent;
using Euclid.Composites.Agent;
using Euclid.Composites.Extensions;
using Euclid.Framework.Cqrs;

namespace Euclid.Composites.Mvc.Binders
{
    public class CommandModelBinder : IEuclidModelBinder
    {
        private readonly IAgentResolutionStrategy[] _resolvers;

        public CommandModelBinder(IAgentResolutionStrategy[] resolvers)
        {
            _resolvers = resolvers;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var agentSystemName = controllerContext.GetAgentSystemName();

            var commandName = controllerContext.GetCommandName();

            var agent = _resolvers.GetAgentInfo(agentSystemName);

            var command = agent.GetCommand(commandName);

            // jt: fill command properties with form values

            return command;
        }

        public bool IsMatch(Type modelType)
        {
            return typeof (ICommand).IsAssignableFrom(modelType);
        }
    }
}