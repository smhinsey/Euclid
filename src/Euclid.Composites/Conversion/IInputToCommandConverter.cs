using System;
using AutoMapper;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Composites.Conversion
{
    public interface IInputToCommandConverter : ITypeConverter<IInputModel, ICommand>
    {
        Type InputModelType { get; }
        Type CommandType { get; }
    }
}