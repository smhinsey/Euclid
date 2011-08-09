using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Framework.Cqrs;

namespace Euclid.Composites.Mvc
{
	public class MvcCompositeAppSettings : CompositeAppSettings
	{
		public MvcCompositeAppSettings()
		{
			Publisher.WithDefault(typeof (DefaultPublisher));
			MessageChannel.WithDefault(typeof (InMemoryMessageChannel));
			PublicationRegistry.WithDefault(typeof (CommandRegistry));
			BlobStorage.WithDefault(typeof (InMemoryBlobStorage));
			MessageSerializer.WithDefault(typeof (JsonMessageSerializer));
			CommandDispatcher.WithDefault(typeof (CommandDispatcher));
			CommandPublicationRecordMapper.WithDefault(typeof (InMemoryCommandPublicationRecordMapper));
		}
	}
}