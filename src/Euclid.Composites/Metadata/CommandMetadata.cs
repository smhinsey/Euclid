using System;
using System.Collections.Generic;
using Euclid.Framework.Cqrs.Metadata;

namespace Euclid.Composites.Metadata
{
	public class CommandMetadata
		: Metadata, ICommandMetadata
	{
		public CommandMetadata(Type commandType) : base(commandType)
		{
			Interfaces = new List<IMetadata>();
			foreach (var inf in Type.GetInterfaces())
			{
				Interfaces.Add(Extract(inf));
			}

			Properties = new List<IPropertyMetadata>();
			foreach (var pi in Type.GetProperties())
			{
				Properties.Add(PropertyMetadata.Extract(pi));
			}
		}

		public IList<IPropertyMetadata> Properties { get; private set; }
	}
}