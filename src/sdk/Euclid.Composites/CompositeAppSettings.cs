using System.Collections.Generic;
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

		public readonly OverridableTypeSetting<IInputModelMapCollection> InputModelCollection;

		public readonly OverridableTypeSetting<IMessageSerializer> MessageSerializer;

		public readonly OverridableTypeSetting<IMessageChannel> OutputChannel;

		public readonly OverridableSetting<string> OutputChannelName;

		public readonly OverridableTypeSetting<IPublicationRegistry<IPublicationRecord, IPublicationRecord>>
			PublicationRegistry;

		public readonly OverridableTypeSetting<IPublisher> Publisher;

		public CompositeAppSettings()
		{
			OutputChannelName = new OverridableSetting<string>();
			OutputChannel = new OverridableTypeSetting<IMessageChannel>("OutputChannel");
			BlobStorage = new OverridableTypeSetting<IBlobStorage>("BlobStorage");
			CommandDispatcher = new OverridableTypeSetting<ICommandDispatcher>("CommandDispatcher");
			CommandPublicationRecordMapper =
				new OverridableTypeSetting<IRecordMapper<CommandPublicationRecord>>("CommandPublicationRecordMapper");
			MessageSerializer = new OverridableTypeSetting<IMessageSerializer>("MessageSerializer");
			PublicationRegistry =
				new OverridableTypeSetting<IPublicationRegistry<IPublicationRecord, IPublicationRecord>>("PublicationRegistry");
			Publisher = new OverridableTypeSetting<IPublisher>("Publisher");
			InputModelCollection = new OverridableTypeSetting<IInputModelMapCollection>("InputModelMapperCollection");

			OutputChannelName.WithDefault("OutputChannelName");
			BlobStorage.WithDefault(typeof(InMemoryBlobStorage));
			CommandDispatcher.WithDefault(typeof(CommandDispatcher));
			CommandPublicationRecordMapper.WithDefault(typeof(InMemoryCommandPublicationRecordMapper));
			MessageSerializer.WithDefault(typeof(JsonMessageSerializer));
			PublicationRegistry.WithDefault(typeof(CommandRegistry));
			Publisher.WithDefault(typeof(DefaultPublisher));
			InputModelCollection.WithDefault(typeof(AutoMapperInputModelCollection));
		}

		public IEnumerable<string> GetInvalidSettingReasons()
		{
			var reasons = new List<string>();
			GetInvalidSettingReason(OutputChannel, reasons);
			GetInvalidSettingReason(BlobStorage, reasons);
			GetInvalidSettingReason(CommandDispatcher, reasons);
			GetInvalidSettingReason(CommandPublicationRecordMapper, reasons);
			GetInvalidSettingReason(MessageSerializer, reasons);
			GetInvalidSettingReason(PublicationRegistry, reasons);
			GetInvalidSettingReason(Publisher, reasons);
			GetInvalidSettingReason(InputModelCollection, reasons);

			return reasons;
		}

		public void Validate()
		{
			OutputChannel.Validate();
			BlobStorage.Validate();
			CommandDispatcher.Validate();
			CommandPublicationRecordMapper.Validate();
			MessageSerializer.Validate();
			PublicationRegistry.Validate();
			Publisher.Validate();
			InputModelCollection.Validate();
		}

		private void GetInvalidSettingReason<T>(OverridableTypeSetting<T> setting, IList<string> reasons)
		{
			if (!setting.IsValid())
			{
				reasons.Add(setting.GetInvalidReason());
			}
		}
	}
}