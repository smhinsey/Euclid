using System;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composites.Conversion
{
    public interface IInputModelTransfomerRegistry
    {
        // jt: is there a need for IModelTransform<TSource, TDest> interface?
        void Add(string partName, IInputToCommandConverter converter);
        IInputModel GetInputModel(string commandName);
        Type GetCommandType(string commandName);
        ICommand GetCommand(IInputModel model);
    }
}