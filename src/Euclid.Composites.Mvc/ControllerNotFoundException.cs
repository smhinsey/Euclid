using System;

namespace Euclid.Composites.Mvc
{
	public class ControllerNotFoundException : Exception
	{
		public ControllerNotFoundException(Uri url) : base(string.Format("No controller found for the URL {0}", url))
		{
		}
	}
}