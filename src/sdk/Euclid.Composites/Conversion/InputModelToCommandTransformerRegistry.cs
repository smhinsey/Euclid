using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Euclid.Common.Logging;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composites.Conversion
{
	public class InputModelToCommandTransformerRegistry : ILoggingSource, IInputModelTransformerRegistry
	{
		private readonly Dictionary<string, IInputToCommandConverter> _inputModelsAndValues =
			new Dictionary<string, IInputToCommandConverter>();

		public void Add(string partName, IInputToCommandConverter converter)
		{
			if (_inputModelsAndValues.ContainsKey(partName))
			{
				throw new PartNameAlreadyRegisteredException(partName);
			}

			var partMetadata = converter.CommandType.GetMetadata();

			_inputModelsAndValues.Add(partName, converter);

			Mapper.CreateMap(converter.InputModelType, partMetadata.Type).ConvertUsing(converter.GetType());
		}

		public ICommand GetCommand(IInputModel model)
		{
			var partName =
				_inputModelsAndValues.Where(row => row.Value.InputModelType == model.GetType()).Select(row => row.Key).
					FirstOrDefault();

			GuardPartNameRegistered(partName);

			var command = Activator.CreateInstance(_inputModelsAndValues[partName].CommandType) as ICommand;

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

		public IPartMetadata GetCommand(Type inputModelType)
		{
			if (!typeof(IInputModel).IsAssignableFrom(inputModelType))
			{
				throw new UnexpectedTypeException(typeof(IInputModel), inputModelType);
			}

			var commandMetadata =
				_inputModelsAndValues.Values.Where(converter => inputModelType == converter.InputModelType).Select(
					partMetadata => partMetadata.CommandType.GetMetadata()).Cast<IPartMetadata>().FirstOrDefault();

			if (commandMetadata == null)
			{
				throw new CommandNotFoundException(inputModelType.Name);
			}

			return commandMetadata;
		}

		public Type GetCommandType(string partName)
		{
			GuardPartNameRegistered(partName);

			return _inputModelsAndValues[partName].CommandType;
		}

		public IInputModel GetInputModel(string partName)
		{
			GuardPartNameRegistered(partName);

			var inputModel = Activator.CreateInstance(_inputModelsAndValues[partName].InputModelType) as IInputModel;

			if (inputModel == null)
			{
				throw new CannotCreateInputModelException(_inputModelsAndValues[partName].InputModelType.Name);
			}

			inputModel.CommandType = _inputModelsAndValues[partName].CommandType;

			inputModel.AgentSystemName = inputModel.CommandType.Assembly.GetAgentMetadata().SystemName;

			return inputModel;
		}

		public IEnumerable<ITypeMetadata> GetInputModels()
		{
			return _inputModelsAndValues.Values.Select(c => c.InputModelType.GetMetadata());
		}

		private void GuardPartNameRegistered(string partName)
		{
			if (!_inputModelsAndValues.ContainsKey(partName))
			{
				throw new InputModelForPartNotRegisteredException(partName);
			}
		}
	}
}