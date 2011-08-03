using System;
using Euclid.Framework.Metadata;

namespace Euclid.Agent.Extensions
{
    public static class TypeExtensions
    {
        public static ITypeMetadata GetMetadata(this Type type)
        {
            return new TypeMetadata(type);
        }
    }
}