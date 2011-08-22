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

		public readonly OverridableTypeSetting<IPublicationRegistry<IPublicationRecord, IPublicationRecord>> PublicationRegistry;

		public readonly OverridableTypeSetting<IPublisher> Publisher = new OverridableTypeSetting<IPublisher>("Publisher");

		public CompositeAppSettings()
		{
			this.BlobStorage = new OverridableTypeSetting<IBlobStorage>("BlobStorage");
			this.CommandDispatcher = new OverridableTypeSetting<ICommandDispatcher>("CommandDispatcher");
			this.CommandPublicationRecordMapper =
				new OverridableTypeSetting<IRecordMapper<CommandPublicationRecord>>("CommandPublicationRecordMapper");
			this.MessageChannel = new OverridableTypeSetting<IMessageChannel>("MessageChannel");
			this.MessageSerializer = new OverridableTypeSetting<IMessageSerializer>("MessageSerializer");
			this.PublicationRegistry =
				new OverridableTypeSetting<IPublicationRegistry<IPublicationRecord, IPublicationRecord>>("PublicationRegistry");
			this.Publisher = new OverridableTypeSetting<IPublisher>("Publisher");

			this.Publisher.WithDefault(typeof(DefaultPublisher));
			this.MessageChannel.WithDefault(typeof(InMemoryMessageChannel));
			this.PublicationRegistry.WithDefault(typeof(CommandRegistry));
			this.BlobStorage.WithDefault(typeof(InMemoryBlobStorage));
			this.MessageSerializer.WithDefault(typeof(JsonMessageSerializer));
			this.CommandDispatcher.WithDefault(typeof(CommandDispatcher));
			this.CommandPublicationRecordMapper.WithDefault(typeof(InMemoryCommandPublicationRecordMapper));
		}
	}
}