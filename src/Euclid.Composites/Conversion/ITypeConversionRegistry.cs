using System;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Conversion
{
    public interface ITypeConversionRegistry<out TDestinationInterface>
    {
        void AddTypeConversion(Type sourceMetadata, Type destinationType);
        void AddTypeConversion<TSource, TDestination>() where TSource : IAgentPart;

        TDestinationInterface Convert(Type nameOfSource);
        TDestinationInterface Convert(ITypeMetadata sourceMetadata);
    }
}