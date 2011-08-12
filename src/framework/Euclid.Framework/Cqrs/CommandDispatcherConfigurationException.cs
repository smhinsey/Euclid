using System;
using System.Collections.Generic;

namespace Euclid.Framework.Cqrs
{
	public class CommandDispatcherConfigurationException : Exception
	{
		public CommandDispatcherConfigurationException(IList<string> errors)
		{
			ConfigurationErrors = errors;
		}

		public IEnumerable<string> ConfigurationErrors { get; private set; }
	}
}