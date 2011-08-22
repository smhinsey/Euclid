﻿using System;
using System.Collections.Generic;
using Euclid.Common.Logging;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Record;
using NHibernate;

namespace Euclid.Common.Storage.NHibernate
{
	public class NhRecordMapper<TRecord> : IRecordMapper<TRecord>, ILoggingSource
		where TRecord : class, IPublicationRecord, new()
	{
		private readonly ISession _session;

		public NhRecordMapper(ISession session)
		{
			this._session = session;
		}

		public TRecord Create(TRecord record)
		{
			this.WriteDebugMessage(string.Format("Creating record {0}({1})", record.GetType().Name, record.Identifier));

			using (var transaction = this._session.BeginTransaction())
			{
				try
				{
					this._session.Save(record);
				}
				catch (Exception e)
				{
					transaction.Rollback();

					throw;
				}

				transaction.Commit();
			}

			this.WriteDebugMessage(string.Format("Created record {0}({1})", record.GetType().Name, record.Identifier));

			return record;
		}

		public TRecord Delete(Guid id)
		{
			var record = this.Retrieve(id);

			using (var transaction = this._session.BeginTransaction())
			{
				try
				{
					if (record == null)
					{
						throw new KeyNotFoundException();
					}

					this.WriteDebugMessage(string.Format("Deleting record {0}({1})", record.GetType().Name, record.Identifier));

					this._session.Delete(record);
				}
				catch (Exception e)
				{
					transaction.Rollback();

					throw;
				}

				transaction.Commit();
			}

			this.WriteDebugMessage(string.Format("Deleted record {0}({1})", record.GetType().Name, record.Identifier));

			return record;
		}

		public TRecord Retrieve(Guid id)
		{
			return this._session.Get<TRecord>(id);
		}

		public TRecord Update(TRecord record)
		{
			this.WriteDebugMessage(string.Format("Updating record {0}({1})", record.GetType().Name, record.Identifier));

			using (var transaction = this._session.BeginTransaction())
			{
				try
				{
					this._session.Update(record, record.Identifier);
				}
				catch (Exception e)
				{
					transaction.Rollback();

					throw;
				}

				transaction.Commit();
			}

			this.WriteDebugMessage(string.Format("Updated record {0}({1})", record.GetType().Name, record.Identifier));

			return this.Retrieve(record.Identifier);
		}
	}
}