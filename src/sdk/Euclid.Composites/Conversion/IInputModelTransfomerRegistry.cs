using System;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composites.Conversion
{
	public interface IInputModelTransfomerRegistry
	{
		void Add(string partName, IInputToCommandConverter converter);
		ICommand GetCommand(IInputModel model);
		Type GetCommandType(string commandName);
		IInputModel GetInputModel(string commandName);
	}
}