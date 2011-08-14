using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Euclid.Framework.Agent.Metadata
{
    public class MethodMetadata : IMethodMetadata
    {
        public MethodMetadata()
        {
            
        }

        public MethodMetadata(MethodInfo mi)
        {
            Name = mi.Name;
            ContainingType = mi.DeclaringType;
            ReturnType = mi.ReturnType;
            Arguments = mi.GetParameters().Select(param=>new ArgumentMetadata
                                                             {
                                                                 DefaultValue = param.RawDefaultValue == DBNull.Value ? null : param.RawDefaultValue,
                                                                 Name = param.Name,
                                                                 Order = param.Position,
                                                                 PropertyType = param.ParameterType
                                                             }).OrderBy(param=>param.Order);
        }

        public string Name { get; set; }
        public Type ContainingType { get; set; }
        public IEnumerable<IArgumentMetadata> Arguments { get; set; }
        public Type ReturnType { get; set; }
    }
}