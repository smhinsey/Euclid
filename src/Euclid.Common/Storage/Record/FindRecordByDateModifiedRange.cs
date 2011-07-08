using System;

namespace Euclid.Common.Storage.Record
{
	public class FindRecordByDateModifiedRange : IQuery
	{
		public DateTime ModifiedOn { get; set; }
		public TimeSpan Range { get; set; }
	}
}