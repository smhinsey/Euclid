using System;

namespace Euclid.Composites
{
	public class QueryNotFoundInCompositeException : Exception
	{
		public QueryNotFoundInCompositeException(string queryName) : base(queryName)
		{
		}
	}
}