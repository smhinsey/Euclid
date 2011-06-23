using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Blob;
using Euclid.Framework.Cqrs.Exceptions;

namespace Euclid.Framework.Cqrs
{
    public class CommandRegistry : PublicationRegistry<CommandPublicationRecord>
    {
        public CommandRegistry(IBasicRecordRepository<CommandPublicationRecord> repository, IBlobStorage blobStorage, IMessageSerializer serializer)
            : base(repository, blobStorage, serializer)
        {
        }
    }
}