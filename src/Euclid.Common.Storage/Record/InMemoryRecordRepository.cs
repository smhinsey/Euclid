using System;
using System.Collections.Concurrent;
using Euclid.Common.Registry;

namespace Euclid.Common.Storage.Record
{
	public class InMemoryRecordRepository<TRecord> : IBasicRecordRepository<TRecord>
		where TRecord : class, IRecord, new()
	{
		protected static readonly ConcurrentDictionary<Guid, TRecord> Records = new ConcurrentDictionary<Guid, TRecord>();


		public TRecord Create(Uri messageLocation, Type messageType)
		{
			var record = new TRecord
			             	{
			             		Identifier = Guid.NewGuid(),
			             		Created = DateTime.Now,
			             		MessageLocation = messageLocation,
			             		MessageType = messageType
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