using System;

namespace Euclid.Common.Storage.Record
{
	public class FindRecordById : IQuery
	{
		public Guid Identifier { get; set; }
	}
}