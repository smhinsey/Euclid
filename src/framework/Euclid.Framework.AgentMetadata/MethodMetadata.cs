using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Euclid.Framework.AgentMetadata
{
	public class MethodMetadata : IMethodMetadata
	{
		public MethodMetadata()
		{
		}

		public MethodMetadata(MethodInfo mi)
		{
			this.Name = mi.Name;
			this.ContainingType = mi.DeclaringType;
			this.ReturnType = mi.ReturnType;
			this.Arguments =
				mi.GetParameters().Select(
					param =>
					new ArgumentMetadata
						{
							DefaultValue = param.RawDefaultValue == DBNull.Value ? null : param.RawDefaultValue, 
							Name = param.Name, 
							Order = param.Position, 
							PropertyType = param.ParameterType
						}).OrderBy(param => param.Order);
		}

		public IEnumerable<IArgumentMetadata> Arguments { get; set; }

		public Type ContainingType { get; set; }

		public string Name { get; set; }

		public Type ReturnType { get; set; }
	}
}