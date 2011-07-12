using System;
using System.Collections.Generic;
using Euclid.Common.Messaging;
using Euclid.Common.Storage.Record;

namespace Euclid.Common.Storage
{
	public class InMemoryRecordFinder<TRecord> : IRecordFinder<TRecord> 
		where TRecord : class, IPublicationRecord, IRecord, new()
	{
		public TRecord FindRecord(IQuery query)
		{
			throw new NotImplementedException();
		}

		public IList<TRecord> FindRecords(IQuery query)
		{
			throw new NotImplementedException();
		}
	}
}