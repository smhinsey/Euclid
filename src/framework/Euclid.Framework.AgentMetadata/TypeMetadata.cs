using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Framework.AgentMetadata.Extensions;

namespace Euclid.Framework.AgentMetadata
{
	public class TypeMetadata : ITypeMetadata
	{
		public TypeMetadata(Type type)
		{
			Namespace = type.Namespace;
			Name = type.Name;
			Type = type;

			Properties = Type.GetProperties().Select(pi => new PropertyMetadata(pi));
			Methods = Type.GetMethods().Where(mi => !mi.IsSpecialName && mi.DeclaringType != typeof (object)).Select(mi => new MethodMetadata(mi));
		}

		protected TypeMetadata()
		{
		}

		public IEnumerable<IMethodMetadata> Methods { get; protected set; }

		public string Name { get; set; }

		public string Namespace { get; protected set; }

		public IEnumerable<IPropertyMetadata> Properties { get; protected set; }

		public Type Type { get; set; }

		public IPartCollection GetContainingPartCollection()
		{
			var agent = Type.Assembly.GetAgentMetadata();

			return agent.GetPartCollectionContainingType(Type);
		}

		public IMetadataFormatter GetFormatter()
		{
			return FormattableMetadataFactory.GetFormatter(this);
		}
	}
}