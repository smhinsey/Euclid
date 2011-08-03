using System;
using System.Web.Mvc;
using Euclid.Composites.AgentResolution;
using Euclid.Composites.Conversion;
using Euclid.Composites.Extensions;
using Euclid.Composites.Mvc.Extensions;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Binders
{
    public class InputModelBinder : IEuclidModelBinder
    {
        private readonly IAgentResolver[] _resolvers;
        private readonly ICommandToIInputModelConversionRegistry _commandToIInputModelConversionRegistry;

        public InputModelBinder(IAgentResolver[] resolvers, ICommandToIInputModelConversionRegistry commandToIInputModelConversionRegistry)
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

            return _commandToIInputModelConversionRegistry.Convert(commandMetadata);
        }

        public bool IsMatch(Type modelType)
        {
            return typeof (IInputModel).IsAssignableFrom(modelType);
        }
    }
}