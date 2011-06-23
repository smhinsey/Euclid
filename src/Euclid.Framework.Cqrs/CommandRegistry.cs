using System;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Blob;
using Euclid.Framework.Cqrs.Exceptions;

namespace Euclid.Framework.Cqrs
{
    public class CommandRegistry : PublicationRegistry<CommandPublicationRecord>, ICommandRegistry
    {
        public CommandRegistry(IBasicRecordRepository<CommandPublicationRecord> repository, IBlobStorage blobStorage, IMessageSerializer serializer)
            : base(repository, blobStorage, serializer)
        {
        }

        public new ICommandPublicationRecord CreateRecord(IMessage message)
        {
            return base.CreateRecord(message);
        }

        public new ICommandPublicationRecord GetRecord(Guid identifier)
        {
            return base.GetRecord(identifier);
        }

        public new ICommandPublicationRecord MarkAsComplete(Guid id)
        {
            return base.MarkAsComplete(id);
        }

        public new ICommandPublicationRecord MarkAsFailed(Guid id, string message, string callStack)
        {
            return base.MarkAsFailed(id, message, callStack);
        }

        public new ICommandPublicationRecord MarkAsUnableToDispatch(Guid id, bool isError, string message)
        {
            return base.MarkAsUnableToDispatch(id, isError, message);
        }
    }
}