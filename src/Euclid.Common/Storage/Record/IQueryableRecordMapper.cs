using System.Collections.Generic;

namespace Euclid.Common.Storage.Record
{
	public interface IQueryableRecordMapper<TRecord>
		where TRecord : IRecord
	{
		TRecord QueryForRecord(IQuery query);
		IList<TRecord> QueryForRecords(IQuery query);
	}
}