using System;
using System.Collections.Generic;
using Euclid.Common.Registry;
using Euclid.Common.Serialization;
using Euclid.Common.Transport;
using NHibernate;

namespace Euclid.Common.Storage.NHibernate
{
	public class NHibernateRecordRepository<TRecord> : IBasicRecordRepository<TRecord>
		where TRecord : class, IRecord, new()
	{
		private readonly IBlobStorage _blobStorage;

		private readonly IMessageSerializer _serializer;
		private readonly ISession _session;

		public NHibernateRecordRepository(IMessageSerializer serializer, IBlobStorage blobStorage, ISession session)
		{
			_serializer = serializer;
			_blobStorage = blobStorage;
			_session = session;
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

			_session.Save(record);

			_session.Flush();

			return record;
		}

		public TRecord Delete(Guid id)
		{
			var record = Retrieve(id);

			if (record == null)
			{
				throw new KeyNotFoundException();
			}

			_session.Delete(record);

			return record;
		}

		public TRecord Retrieve(Guid id)
		{
			return _session.Get<TRecord>(id);
		}

		public TRecord Update(TRecord record)
		{
			_session.Update(record, record.Identifier);

			return Retrieve(record.Identifier);
		}
	}
}