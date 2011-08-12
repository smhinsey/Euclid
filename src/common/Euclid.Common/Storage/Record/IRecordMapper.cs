using System;

namespace Euclid.Common.Storage.Record
{
	public interface IRecordMapper<TRecord>
	{
		TRecord Create(TRecord record);
		TRecord Delete(Guid id);
		TRecord Retrieve(Guid id);
		TRecord Update(TRecord record);
	}
}