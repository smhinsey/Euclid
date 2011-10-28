using System;
using Euclid.Framework.AgentMetadata.PartCollection;

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

		public static IPartMetadata GetPartMetadata(this Type type)
		{
			if (!typeof(IAgentPart).IsAssignableFrom(type))
			{
				throw new InvalidAgentPartImplementationException(type);
			}

			return new PartMetadata(type);
		}
	}
}