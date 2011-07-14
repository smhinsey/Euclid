﻿using System;
using System.Collections.Concurrent;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Record;

namespace Euclid.Common.Storage
{
	public class InMemoryRecordMapper<TRecord> : IRecordMapper<TRecord>
		where TRecord : class, IPublicationRecord, new()
	{
		protected static readonly ConcurrentDictionary<Guid, TRecord> Records = new ConcurrentDictionary<Guid, TRecord>();

		public TRecord Create(TRecord record)
		{
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