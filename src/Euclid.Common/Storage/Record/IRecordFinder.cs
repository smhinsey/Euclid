using System.Collections.Generic;

namespace Euclid.Common.Storage.Record
{
	public interface IRecordFinder<TRecord>
		where TRecord : IRecord
	{
		TRecord FindRecord(IQuery query);
		IList<TRecord> FindRecords(IQuery query);
	}
}