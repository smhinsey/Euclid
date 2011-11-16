using System;
using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composites
{
	public interface IInputModelMapCollection
	{
		IEnumerable<IPartMetadata> Commands { get; }

		IEnumerable<ITypeMetadata> InputModels { get; }

		bool CommandIsMapped<TCommand>() where TCommand : ICommand;

		ICommand GetCommand<TSourceInputModel>(TSourceInputModel inputModel) where TSourceInputModel : IInputModel;

		IPartMetadata GetCommandMetadataForInputModel(Type inputModelType);

		IPartMetadata GetCommandMetadataForInputModel<TSourceInputModel>() where TSourceInputModel : IInputModel;

		IPartMetadata GetCommandMetadataForInputModel(IInputModel model);

		Type GetInputModelTypeForCommandName(string commandName);

		bool InputModelIsMapped<TInputModel>() where TInputModel : IInputModel;

		void RegisterInputModel<TSourceInputModel, TDestinationCommand>() where TSourceInputModel : IInputModel
			where TDestinationCommand : ICommand;

		void RegisterInputModel<TSourceInputModel, TDestinationCommand>(
			Func<TSourceInputModel, TDestinationCommand> customMap) where TSourceInputModel : IInputModel
			where TDestinationCommand : ICommand;
	}
}