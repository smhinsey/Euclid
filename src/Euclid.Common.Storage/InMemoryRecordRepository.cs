using System;
using System.Collections.Concurrent;
using Euclid.Common.Registry;
using Euclid.Common.Transport;

namespace Euclid.Common.Storage
{
	public class InMemoryRecordRepository<TRecord, TMessage> : IBasicRecordRepository<TRecord, TMessage>
		where TRecord : IRecord<TMessage>, new()
		where TMessage : IMessage
	{
		protected static readonly ConcurrentDictionary<Guid, TRecord> Records = new ConcurrentDictionary<Guid, TRecord>();

		public TRecord Create(TMessage message)
		{
			var record = new TRecord
			             	{
			             		Identifier = Guid.NewGuid(),
			             		Message = message,
			             		Created = DateTime.Now
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