using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Euclid.Common.Configuration;
using Euclid.Composites.Conversion;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.AgentMetadata.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composites
{
	public class AutoMapperInputModelCollection : IInputModelMapCollection
	{
		public AutoMapperInputModelCollection()
		{
			Mapper.Reset();
		}

		public void RegisterInputModel<TSourceInputModel, TDestinationCommand>() where TSourceInputModel : IInputModel where TDestinationCommand : ICommand
		{
			RegisterInputModel<TSourceInputModel, TDestinationCommand>(null);
		}

		public void RegisterInputModel<TSourceInputModel, TDestinationCommand>(Func<TSourceInputModel, TDestinationCommand> customMap) where TSourceInputModel : IInputModel where TDestinationCommand : ICommand
		{
			if (InputModelIsMapped<TSourceInputModel>())
			{
				throw new InputModelAlreadyRegisteredException(typeof(TSourceInputModel).FullName);
			}

			if (CommandIsMapped<TDestinationCommand>())
			{
				throw new CommandAlreadyMappedException(typeof(TDestinationCommand).FullName);
			}

			var expression = Mapper.CreateMap<TSourceInputModel, TDestinationCommand>();
			if (customMap != null)
			{
				expression.ConvertUsing(customMap);
			}
		}

		public IPartMetadata GetCommandMetadataForInputModel(Type inputModelType)
		{
			if (!typeof(IInputModel).IsAssignableFrom(inputModelType))
			{
				throw new InvalidTypeSettingException(inputModelType.FullName, typeof(IInputModel), inputModelType.GetType());
			}

			var map = Mapper.GetAllTypeMaps().Where(m => m.SourceType == inputModelType).FirstOrDefault();

			if (map == null)
			{
				throw new InputModelNotRegisteredException(inputModelType);
			}

			var commandType = map.DestinationType;

			return commandType.GetPartMetadata();
		}

		public IPartMetadata GetCommandMetadataForInputModel(IInputModel model)
		{
			return GetCommandMetadataForInputModel(model.GetType());
		}

		public IPartMetadata GetCommandMetadataForInputModel<TInputModel>() where TInputModel : IInputModel
		{
			return GetCommandMetadataForInputModel(typeof (TInputModel));
		}

		public ICommand GetCommand<TSourceInputModel>(TSourceInputModel inputModel) where TSourceInputModel : IInputModel
		{
			var commandMetadata = GetCommandMetadataForInputModel<TSourceInputModel>();

			return Mapper.Map(inputModel, inputModel.GetType(), commandMetadata.Type) as ICommand;
		}

		public bool InputModelIsMapped<TInputModel>() where TInputModel : IInputModel
		{
			return Mapper.GetAllTypeMaps().Any(m => m.SourceType== typeof(TInputModel));
		}

		public bool CommandIsMapped<TCommand>() where TCommand : ICommand
		{
			return Mapper.GetAllTypeMaps().Any(m => m.DestinationType == typeof (TCommand));
		}

		public IEnumerable<ITypeMetadata> InputModels
		{
			get { return Mapper.GetAllTypeMaps().Where(m => typeof(IInputModel).IsAssignableFrom(m.SourceType)).Select(m=>m.SourceType.GetMetadata()); }
		}

		public IEnumerable<IPartMetadata> Commands
		{
			get { return Mapper.GetAllTypeMaps().Where(m => typeof(ICommand).IsAssignableFrom(m.DestinationType)).Select(m => m.DestinationType.GetPartMetadata()); }
		}

		public Type GetInputModelTypeForCommandName(string commandName)
		{
			var type = Mapper.GetAllTypeMaps().Where(t => t.DestinationType.Name == commandName).Select(t=>t.SourceType).FirstOrDefault();

			if (type == null)
			{
				throw new CannotMapCommandException(commandName, InputModels.Select(m => m.Type.FullName));
			}

			return type;
		}
	}
}