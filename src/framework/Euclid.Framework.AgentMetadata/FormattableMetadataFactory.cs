using System;
using Euclid.Framework.AgentMetadata.Formatters;
using Euclid.Framework.Cqrs;
using Euclid.Framework.Models;
using Newtonsoft.Json;

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
            else if (typeof(IInputModel).IsAssignableFrom(metadata.Type))
            {
                return new InputModelFormatter(metadata);
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

    public class InputModelFormatter : MetadataFormatter, IMetadataFormatter
    {
        private readonly ITypeMetadata _inputModelMetadata;

        public InputModelFormatter(ITypeMetadata inputModelMetadata)
        {
            _inputModelMetadata = inputModelMetadata;
        }

        protected override string GetAsXml()
        {
            throw new NotImplementedException();
        }

        protected override object GetJsonObject(JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}