using System;

namespace Euclid.Framework.AgentMetadata.Extensions
{
	public static class TypeExtensions
	{
		public static ITypeMetadata GetMetadata(this Type type)
		{
			return new TypeMetadata(type);
		}
	}
}