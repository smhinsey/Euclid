using System;

namespace Euclid.Common.Storage.Record
{
	public class FindRecordByDateCreated : IQuery
	{
		public DateTime CreatedOn { get; set; }
	}
}