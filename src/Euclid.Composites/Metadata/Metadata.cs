using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Metadata
{
	public class Metadata : IMetadata
	{
		public Metadata(Type t)
		{
			Name = t.Name;
			Type = t;
		}

		public IList<IMetadata> Interfaces { get; protected set; }
		public string Name { get; set; }

		public string Namespace
		{
			get { return (Type == null) ? string.Empty : Type.Namespace; }
		}

		public Type Type { get; set; }

		internal static IMetadata Extract(Type t)
		{
			return new Metadata(t);
		}
	}
}