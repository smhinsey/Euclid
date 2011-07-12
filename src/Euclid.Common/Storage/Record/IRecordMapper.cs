using System;

namespace Euclid.Common.Storage.Record
{
	public interface IRecordMapper<TRecord>
	{
		TRecord Create(Uri messageLocation, Type messageType);
		TRecord Delete(Guid id);
		TRecord Retrieve(Guid id);
		TRecord Update(TRecord record);
	}
}