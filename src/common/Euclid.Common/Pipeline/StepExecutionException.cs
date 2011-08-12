using System;

namespace Euclid.Common.Pipeline
{
	public class StepExecutionException : Exception
	{
		private readonly object _dataToProcess;
		private readonly Type _stepType;

		public StepExecutionException(object dataToProcess, Type stepType, Exception exception)
			: base(string.Format("An error occurred executing the step {0}", stepType.FullName), exception)
		{
			_dataToProcess = dataToProcess;
			_stepType = stepType;
		}
	}
}