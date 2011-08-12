using System;

namespace Euclid.Composites.Mvc.Extensions
{
	public class RequiredRouteDataMissingException : Exception
	{
		public RequiredRouteDataMissingException(string key)
			: base(string.Format("There was no route data for key: {0}", key))
		{
		}
	}
}