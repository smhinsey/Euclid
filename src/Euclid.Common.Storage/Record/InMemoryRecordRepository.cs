using System;
using System.Collections.Concurrent;
using Euclid.Common.Registry;
using Euclid.Common.Serialization;
using Euclid.Common.Transport;

namespace Euclid.Common.Storage.Record
{
	public class InMemoryRecordRepository<TRecord> : IBasicRecordRepository<TRecord>
		where TRecord : class, IRecord, new()
	{
		protected static readonly ConcurrentDictionary<Guid, TRecord> Records = new ConcurrentDictionary<Guid, TRecord>();

		private readonly IBlobStorage _blobStorage;

		private readonly IMessageSerializer _serializer;

		public InMemoryRecordRepository(IBlobStorage blobStorage, IMessageSerializer serializer)
		{
			_blobStorage = blobStorage;
			_serializer = serializer;
		}

		public TRecord Create(IMessage message)
		{
			var msg = _serializer.Serialize(message);

			var uri = _blobStorage.Put(msg, message.GetType().FullName, null);

			var record = new TRecord
			             	{
			             		Identifier = Guid.NewGuid(),
			             		Created = DateTime.Now,
			             		MessageLocation = uri,
			             		MessageType = message.GetType()
			             	};

			Records.TryAdd(record.Identifier, record);

			return record;
		}

		public TRecord Delete(Guid id)
		{
			TRecord record;

			Records.TryRemove(id, out record);

			return record;
		}

		public TRecord Retrieve(Guid id)
		{
			TRecord record;

			Records.TryGetValue(id, out record);

			return record;
		}

		public TRecord Update(TRecord record)
		{
			Records.TryUpdate(record.Identifier, record, Records[record.Identifier]);

			return record;
		}
	}
}