using System;
using System.Web.Mvc;
using Euclid.Agent.Commands;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Conversion;
using Euclid.Composites.Extensions;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Mvc.Binders
{
    public class CommandModelBinder : IEuclidModelBinder
    {
        private readonly IAgentResolutionStrategy[] _resolvers;
        private readonly ICommandToIInputModelConversionRegistry _commandToIInputModelConversionRegistry;

        public CommandModelBinder(IAgentResolutionStrategy[] resolvers, ICommandToIInputModelConversionRegistry commandToIInputModelConversionRegistry)
        {
            _resolvers = resolvers;
            _commandToIInputModelConversionRegistry = commandToIInputModelConversionRegistry;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var agentSystemName = controllerContext.GetAgentSystemName();

            var commandName = controllerContext.GetCommandName();

            var agent = _resolvers.GetAgentMetadata(agentSystemName);

            var commandMetadata = agent.Commands.GetMetadata(commandName);

            var inputModel = _commandToIInputModelConversionRegistry.Convert(commandMetadata);

            return CommandFactory.Get(commandMetadata, inputModel, controllerContext.HttpContext.Request.Form);
        }

        public bool IsMatch(Type modelType)
        {
            return typeof (ICommand).IsAssignableFrom(modelType);
        }
    }
}