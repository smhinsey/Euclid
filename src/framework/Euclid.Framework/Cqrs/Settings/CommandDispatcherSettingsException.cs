using System;
using System.Collections.Generic;

namespace Euclid.Framework.Cqrs.Settings
{
	public class CommandDispatcherSettingsException : Exception
	{
		public CommandDispatcherSettingsException(IList<string> errors)
		{
			ConfigurationErrors = errors;
		}

		public IEnumerable<string> ConfigurationErrors { get; private set; }
	}
}