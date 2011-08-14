using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Euclid.Agent.Extensions;
using Euclid.Common.Logging;
using Euclid.Framework.Agent.Metadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composites.Conversion
{
	public class InputModelToCommandTransformerRegistry : IInputModelTransfomerRegistry, ILoggingSource
	{
		private readonly Dictionary<string, Tuple<Type, ITypeMetadata>> _inputModelsAndValues =
			new Dictionary<string, Tuple<Type, ITypeMetadata>>();

		public void Add(string partName, IInputToCommandConverter converter)
		{
			if (_inputModelsAndValues.ContainsKey(partName))
			{
				throw new PartNameAlreadyRegisteredException(partName);
			}

			var partMetadata = converter.CommandType.GetMetadata();

			_inputModelsAndValues.Add(partName, new Tuple<Type, ITypeMetadata>(converter.InputModelType, partMetadata));

			Mapper.CreateMap(converter.InputModelType, partMetadata.Type).ConvertUsing(converter.GetType());
		}

		public ICommand GetCommand(IInputModel model)
		{
			var partName = _inputModelsAndValues
				.Where(row => row.Value.Item1 == model.GetType())
				.Select(row => row.Key)
				.FirstOrDefault();

			GuardPartNameRegistered(partName);

			var command = Activator.CreateInstance(_inputModelsAndValues[partName].Item2.Type) as ICommand;

			if (command == null)
			{
				throw new CannotCreateCommandException();
			}

			command = Mapper.Map(model, command, model.GetType(), command.GetType()) as ICommand;

			if (command == null)
			{
				throw new CannotMapCommandException();
			}

			command.Created = DateTime.Now;

			command.Identifier = Guid.NewGuid();

			return command;
		}

		public Type GetCommandType(string partName)
		{
			GuardPartNameRegistered(partName);

			return _inputModelsAndValues[partName].Item2.Type;
		}

		public IInputModel GetInputModel(string partName)
		{
			GuardPartNameRegistered(partName);

			return Activator.CreateInstance(_inputModelsAndValues[partName].Item1) as IInputModel;
		}

		private void GuardPartNameRegistered(string partName)
		{
			if (!_inputModelsAndValues.ContainsKey(partName))
			{
			    throw new InputModelForPartNotRegisteredException(partName);
			}
		}
	}

    internal class InputModelForPartNotRegisteredException : Exception
    {
        public InputModelForPartNotRegisteredException(string partName)  :
            base(string.Format("There are no input models associated with the command '{0}'", partName))
        {
        }
    }
}