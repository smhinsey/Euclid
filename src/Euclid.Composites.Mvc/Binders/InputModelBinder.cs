using System;
using System.Web.Mvc;
using Euclid.Composites.Agent;
using Euclid.Composites.Extensions;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Binders
{
    public class InputModelBinder : IEuclidModelBinder
    {
        private readonly IAgentResolutionStrategy[] _resolvers;
        private readonly ICommandToInputModelConversionRegistry _commandToInputModelConversionRegistry;

        public InputModelBinder(IAgentResolutionStrategy[] resolvers, ICommandToInputModelConversionRegistry commandToInputModelConversionRegistry)
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

            return _commandToInputModelConversionRegistry.Convert(commandMetadata);
        }

        public bool IsMatch(Type modelType)
        {
            return typeof (IInputModel).IsAssignableFrom(modelType);
        }
    }
}