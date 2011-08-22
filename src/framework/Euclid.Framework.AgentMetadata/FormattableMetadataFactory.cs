using System;
using Euclid.Framework.AgentMetadata.Formatters;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;

namespace Euclid.Framework.AgentMetadata
{
	public class FormattableMetadataFactory
	{
		public static IMetadataFormatter GetFormatter(Type metadataSource)
		{
			var metadata = new TypeMetadata(metadataSource);

			return GetFormatter(metadata);
		}

		public static IMetadataFormatter GetFormatter(ITypeMetadata metadata)
		{
			if (typeof(ICommand).IsAssignableFrom(metadata.Type))
			{
				return new CommandFormatter(metadata);
			}
			else if (typeof(IReadModel).IsAssignableFrom(metadata.Type))
			{
				return new ReadModelFormatter(metadata);
			}
			else if (typeof(IQuery).IsAssignableFrom(metadata.Type))
			{
				return new QueryFormatter(metadata);
			}

			throw new AgentPartFormatterNotFoundException(metadata.Type.Name);
		}

		public static IMetadataFormatter GetFormatter(IPartCollection metadata)
		{
			if (typeof(ICommand).IsAssignableFrom(metadata.CollectionType))
			{
				return new CommandCollectionFormatter(metadata);
			}
			else if (typeof(IReadModel).IsAssignableFrom(metadata.CollectionType))
			{
				return new ReadModelCollectionFormatter(metadata);
			}
			else if (typeof(IQuery).IsAssignableFrom(metadata.CollectionType))
			{
				return new QueryCollectionFormatter(metadata);
			}

			throw new AgentPartFormatterNotFoundException(metadata.CollectionType.Name);
		}
	}
}