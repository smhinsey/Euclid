using System;

namespace Euclid.Framework.AgentMetadata.Extensions
{
	public static class TypeExtensions
	{
		public static ITypeMetadata GetMetadata(this Type type)
		{
            if (typeof(IAgentPart).IsAssignableFrom(type))
            {
                return new PartMetadata(type);
            }

			return new TypeMetadata(type);
		}
	}
}