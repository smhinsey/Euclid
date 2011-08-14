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
			using (var transaction = _session.BeginTransaction())
			{
				try
				{
					_session.Save(record);
				}
				catch (Exception e)
				{
					transaction.Rollback();

					throw;
				}

				transaction.Commit();
			}

			return record;
		}

		public TRecord Delete(Guid id)
		{
			var record = Retrieve(id);

			using (var transaction = _session.BeginTransaction())
			{
				try
				{
					if (record == null)
					{
						throw new KeyNotFoundException();
					}

					_session.Delete(record);
				}
				catch (Exception e)
				{
					transaction.Rollback();

					throw;
				}

				transaction.Commit();
			}

			return record;
		}

		public TRecord Retrieve(Guid id)
		{
			return _session.Get<TRecord>(id);
		}

		public TRecord Update(TRecord record)
		{
			using (var transaction = _session.BeginTransaction())
			{
				try
				{
					_session.Update(record, record.Identifier);
				}
				catch (Exception e)
				{
					transaction.Rollback();

					throw;
				}

				transaction.Commit();
			}

			return Retrieve(record.Identifier);
		}
	}
}