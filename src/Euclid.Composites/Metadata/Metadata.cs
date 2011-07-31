using System;
using System.Collections.Generic;
using Euclid.Framework.Metadata;

namespace Euclid.Composites.Metadata
{
	public class Metadata : IMetadataOriginal
	{
		public Metadata(Type t)
		{
			Name = t.Name;
			Type = t;
		}

		public IList<IMetadataOriginal> Interfaces { get; protected set; }
		public string Name { get; set; }

		public string Namespace
		{
			get { return (Type == null) ? string.Empty : Type.Namespace; }
		}

		public Type Type { get; set; }

		internal static IMetadataOriginal Extract(Type t)
		{
			return new Metadata(t);
		}
	}
}