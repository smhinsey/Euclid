using System;
using System.Web.Mvc;
using Euclid.Composites.Agent;
using Euclid.Composites.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Mvc.Binders
{
    public class CommandModelBinder : IEuclidModelBinder
    {
        private readonly IAgentResolutionStrategy[] _resolvers;
        private readonly ICommandToInputModelConversionRegistry _commandToInputModelConversionRegistry;

        public CommandModelBinder(IAgentResolutionStrategy[] resolvers, ICommandToInputModelConversionRegistry commandToInputModelConversionRegistry)
        {
            _resolvers = resolvers;
            _commandToInputModelConversionRegistry = commandToInputModelConversionRegistry;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var agentSystemName = controllerContext.GetAgentSystemName();

            var commandName = controllerContext.GetCommandName();

            var agent = _resolvers.GetAgentMetadata(agentSystemName);

            var commandMetadata = agent.Commands.GetMetadata(commandName);

            var inputModel = _commandToInputModelConversionRegistry.Convert(commandMetadata);

            return CommandFactory.Get(commandMetadata, inputModel, controllerContext.HttpContext.Request.Form);
        }

        public bool IsMatch(Type modelType)
        {
            return typeof (ICommand).IsAssignableFrom(modelType);
        }
    }
}