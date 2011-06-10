using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Transport;

namespace Euclid.Common.Registry
{
    public class InMemoryRegistry<T> : IRegistry<T> where T : IRecord, IMessage, new()
    {
        private static Dictionary<Guid, T> _records;

        public InMemoryRegistry()
        {
            _records = new Dictionary<Guid, T>();
        }

        public void Add(T record)
        {
            if (!_records.ContainsKey(record.Identifier))
            {
                _records.Add(record.Identifier, record);
            }
        }

        public T Get(Guid id)
        {
            var deletedRecord = default(T);

            if (_records.ContainsKey(id))
            {
                deletedRecord = _records[id];
                _records.Remove(id);
            }

            return deletedRecord;
        }
    }
}
