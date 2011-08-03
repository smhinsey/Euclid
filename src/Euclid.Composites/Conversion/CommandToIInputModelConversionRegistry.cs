using System;
using System.Collections.Generic;
using Euclid.Agent.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Metadata;
using Euclid.Framework.Models;

namespace Euclid.Composites.Conversion
{
    public interface ICommandToIInputModelConversionRegistry : ITypeConversionRegistry<IInputModel>
    {
    }

    public class CommandToIInputModelConversionRegistry : ICommandToIInputModelConversionRegistry
    {
        private readonly IDictionary<string, Type> _commandToInputMapType = new Dictionary<string, Type>();

        public void AddTypeConversion(Type commandType, Type inputMapType)
        {
            var commandMetadata = commandType.GetMetadata();

            GuardCommandMetadata(commandMetadata);

            GuardInputModel(inputMapType);

            _commandToInputMapType.Add(commandMetadata.Name, inputMapType);
        }

        public void AddTypeConversion<TSource, TDestionation>() where TSource : IAgentPart
        {
            AddTypeConversion(typeof (TSource), typeof (TDestionation));
        }

        public IInputModel Convert(Type commandType)
        {
            var commandMeadata = commandType.GetMetadata();

            GuardCommandName(commandMeadata.Name);

            var model = Activator.CreateInstance(_commandToInputMapType[commandMeadata.Name]) as IInputModel;

            if (model == null)
            {
                throw new CannotCreateInputModelException(commandMeadata.Name);
            }

            return model;
        }

        public IInputModel Convert(ITypeMetadata commandMetadata)
        {
            return Convert(commandMetadata.Type);
        }

        private void GuardInputModel(Type inputModelType)
        {
            if (!typeof (IInputModel).IsAssignableFrom(inputModelType))
            {
                throw new UnexpectedTypeException(typeof(IInputModel), inputModelType);
            }
        }

        private void GuardCommandMetadata(ITypeMetadata commandMetadata)
        {
            if (!typeof (ICommand).IsAssignableFrom(commandMetadata.Type))
            {
                throw new UnexpectedTypeException(typeof(ICommand), commandMetadata.Type);
            }

            if (_commandToInputMapType.ContainsKey(commandMetadata.Name))
            {
                throw new CommandAlreadyMappedException(commandMetadata.Type);
            }
        }

        private void GuardCommandName(string commandName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("commandName");
            }

            if (!_commandToInputMapType.ContainsKey(commandName))
            {
                throw new CommandNotRegisteredException(commandName);
            }
        }
    }
}