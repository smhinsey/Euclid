using System;

namespace Euclid.Common.Storage.Record
{
	public class FindRecordByDateModified : IQuery
	{
		public DateTime ModifiedOn { get; set; }
	}
}