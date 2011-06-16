using System;
using System.IO;
using Euclid.Common.Serialization;
using Euclid.Common.Storage;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
    public class DefaultRecordRegistry<TRecord> : IRegistry<TRecord>
        where TRecord : class, IRecord, new()
    {
        private readonly IBasicRecordRepository<TRecord> _repository;

        private readonly IBlobStorage _blobStorage;

        private readonly IMessageSerializer _serializer;


        public DefaultRecordRegistry(IBasicRecordRepository<TRecord> repository, IBlobStorage blobStorage, IMessageSerializer serializer)
        {
            _repository = repository;
            _blobStorage = blobStorage;
            _serializer = serializer;
        }

        public TRecord CreateRecord(IMessage message)
        {
            var msg = _serializer.Serialize(message);

            var uri = _blobStorage.Put(msg, message.GetType().FullName, null);

            return _repository.Create(uri, message.GetType());
        }

        public TRecord Get(Guid id)
        {
            return _repository.Retrieve(id);
        }

        public IMessage GetMessage(TRecord record)
        {
            var messgaeBytes = _blobStorage.Get(record.MessageLocation);

            var messageStream = new MemoryStream(messgaeBytes);

            return Convert.ChangeType(_serializer.Deserialize(messageStream), record.MessageType) as IMessage;
        }

        public TRecord MarkAsComplete(Guid id)
        {
            return UpdateRecord(id, r => r.Completed = true);
        }

        public TRecord MarkAsFailed(Guid id, string message, string callStack)
        {
            return UpdateRecord
                (id, r =>
                        {
                            r.Completed = true;
                            r.Error = true;
                            r.ErrorMessage = message;
                            r.CallStack = callStack;
                        });
        }

        private TRecord UpdateRecord(Guid id, Action<TRecord> actOnRecord)
        {
            var record = Get(id);

            actOnRecord(record);

            return _repository.Update(record);
        }
    }
}