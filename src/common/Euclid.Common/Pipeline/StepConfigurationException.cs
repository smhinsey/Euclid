using System;

namespace Euclid.Common.Pipeline
{
	public class StepConfigurationException : Exception
	{
		public StepConfigurationException(string msg)
			: base(msg)
		{
		}

		public StepConfigurationException()
		{
		}
	}
}