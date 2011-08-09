using Euclid.Common.Configuration;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;
using Euclid.Framework.Cqrs;

namespace Euclid.Composites
{
	public class CompositeAppSettings : IOverridableSettings
	{
		public readonly OverridableTypeSetting<IBlobStorage> BlobStorage;
		public readonly OverridableTypeSetting<ICommandDispatcher> CommandDispatcher;
		public readonly OverridableTypeSetting<IRecordMapper<CommandPublicationRecord>> CommandPublicationRecordMapper;
		public readonly OverridableTypeSetting<IMessageChannel> MessageChannel;
		public readonly OverridableTypeSetting<IMessageSerializer> MessageSerializer;
		public readonly OverridableTypeSetting<IPublicationRegistry<IPublicationRecord>> PublicationRegistry;
		public readonly OverridableTypeSetting<IPublisher> Publisher = new OverridableTypeSetting<IPublisher>("Publisher");

		public CompositeAppSettings()
		{
			BlobStorage = new OverridableTypeSetting<IBlobStorage>("BlobStorage");
			CommandDispatcher = new OverridableTypeSetting<ICommandDispatcher>("CommandDispatcher");
			CommandPublicationRecordMapper = new OverridableTypeSetting<IRecordMapper<CommandPublicationRecord>>("CommandPublicationRecordMapper");
			MessageChannel = new OverridableTypeSetting<IMessageChannel>("MessageChannel");
			MessageSerializer = new OverridableTypeSetting<IMessageSerializer>("MessageSerializer");
			PublicationRegistry = new OverridableTypeSetting<IPublicationRegistry<IPublicationRecord>>("PublicationRegistry");
			Publisher = new OverridableTypeSetting<IPublisher>("Publisher");

			Publisher.WithDefault(typeof(DefaultPublisher));
			MessageChannel.WithDefault(typeof(InMemoryMessageChannel));
			PublicationRegistry.WithDefault(typeof(CommandRegistry));
			BlobStorage.WithDefault(typeof(InMemoryBlobStorage));
			MessageSerializer.WithDefault(typeof(JsonMessageSerializer));
			CommandDispatcher.WithDefault(typeof(CommandDispatcher));
			CommandPublicationRecordMapper.WithDefault(typeof(InMemoryCommandPublicationRecordMapper));
		}
	}
}