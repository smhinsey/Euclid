using System;
using System.Collections.Generic;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
	public class InMemoryRegistry<TRecord, TMessage> : IRegistry<TRecord, TMessage> where TRecord : IRecord<TMessage>, new()
	                                                                    where TMessage : IMessage
	{
		private static Dictionary<Guid, TRecord> _records;

		protected InMemoryRegistry()
		{
			_records = new Dictionary<Guid, TRecord>();
		}

		public void Add(TRecord record)
		{
			if (!_records.ContainsKey(record.Identifier))
			{
				_records.Add(record.Identifier, record);
			}
		}

		public TRecord CreateRecord(TMessage message)
		{
			return new TRecord
			       	{
			       		Message = message,
			       		Identifier = Guid.NewGuid(),
			       		Created = DateTime.Now
			       	};
		}

		public TRecord Get(Guid id)
		{
			var deletedRecord = default(TRecord);

			if (_records.ContainsKey(id))
			{
				deletedRecord = _records[id];
				_records.Remove(id);
			}

			return deletedRecord;
		}
	}
}