using System;
using System.Collections.Generic;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Record;
using NHibernate;

namespace Euclid.Common.Storage.NHibernate
{
	public class NhRecordMapper<TRecord> : IRecordMapper<TRecord>
		where TRecord : class, IPublicationRecord, new()
	{
		private readonly ISession _session;

		public NhRecordMapper(ISession session)
		{
			_session = session;
		}

		public TRecord Create(TRecord record)
		{
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