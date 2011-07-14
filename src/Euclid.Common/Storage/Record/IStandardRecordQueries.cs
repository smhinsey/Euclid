using System;
using System.Collections.Generic;

namespace Euclid.Common.Storage.Record
{
	public interface IStandardRecordQueries : IQuery
	{
		IList<IRecord> FindByCreationDate(DateTime specificDate);
		IList<IRecord> FindByCreationDate(DateTime from, DateTime to);
		IList<IRecord> FindByModificationDate(DateTime specificDate);
		IList<IRecord> FindByModificationDate(DateTime from, DateTime to);
		IRecord FindById(Guid id);
	}
}