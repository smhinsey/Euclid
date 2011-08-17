using Euclid.Common.Messaging;
using Euclid.Common.Storage.Binary;
using Euclid.Common.Storage.Record;

namespace Euclid.Framework.Cqrs
{
	public class CommandRegistry : PublicationRegistry<CommandPublicationRecord, ICommandPublicationRecord>, ICommandRegistry
	{
		public CommandRegistry(IRecordMapper<CommandPublicationRecord> mapper, IBlobStorage blobStorage, IMessageSerializer serializer)
			: base(mapper, blobStorage, serializer)
		{
		}
	}
}