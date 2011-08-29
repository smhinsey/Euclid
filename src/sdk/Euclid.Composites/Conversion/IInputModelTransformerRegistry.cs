using System;
using System.Collections.Generic;
using Euclid.Framework.AgentMetadata;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composites.Conversion
{
	public interface IInputModelTransformerRegistry
	{
		void Add(string partName, IInputToCommandConverter converter);

		ICommand GetCommand(IInputModel model);

		IPartMetadata GetCommand(Type t);

		Type GetCommandType(string commandName);

		IInputModel GetInputModel(string commandName);

		IEnumerable<ITypeMetadata> GetInputModels();
	}
}