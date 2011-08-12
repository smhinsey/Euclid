using System;
using Euclid.Framework.Agent.Metadata;

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