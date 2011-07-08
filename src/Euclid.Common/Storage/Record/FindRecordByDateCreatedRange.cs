using System;

namespace Euclid.Common.Storage.Record
{
	public class FindRecordByDateCreatedRange : IQuery
	{
		public DateTime CreatedOn { get; set; }
		public TimeSpan Range { get; set; }
	}
}