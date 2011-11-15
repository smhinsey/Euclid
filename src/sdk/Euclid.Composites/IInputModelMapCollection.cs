using System;
using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composites
{
	public interface IInputModelMapCollection
	{
		void RegisterInputModel<TSourceInputModel, TDestinationCommand>()
			where TSourceInputModel : IInputModel
			where TDestinationCommand : ICommand;

		void RegisterInputModel<TSourceInputModel, TDestinationCommand>(Func<TSourceInputModel, TDestinationCommand> customMap)
			where TSourceInputModel : IInputModel
			where TDestinationCommand : ICommand;

		Type GetInputModelTypeForCommandName(string commandName);

		IPartMetadata GetCommandMetadataForInputModel(Type inputModelType);

		IPartMetadata GetCommandMetadataForInputModel<TSourceInputModel>()
			where TSourceInputModel : IInputModel;

		IPartMetadata GetCommandMetadataForInputModel(IInputModel model);

		ICommand GetCommand<TSourceInputModel>(TSourceInputModel inputModel) where TSourceInputModel : IInputModel;

		bool InputModelIsMapped<TInputModel>() where TInputModel : IInputModel;

		bool CommandIsMapped<TCommand>() where TCommand : ICommand;

		IEnumerable<ITypeMetadata> InputModels { get; }

		IEnumerable<IPartMetadata> Commands { get; }
	}
}