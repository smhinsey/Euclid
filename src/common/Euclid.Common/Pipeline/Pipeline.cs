using System;
using System.Collections.Generic;
using System.Linq;

namespace Euclid.Common.Pipeline
{
	public class Pipeline<T>
	{
		private readonly SortedList<int, IPipelineStep<T>> _steps = new SortedList<int, IPipelineStep<T>>();

		public void Configure(params IPipelineStep<T>[] steps)
		{
			GuardAgainstNullSteps(steps);
			GuardAgainstMultiple(steps, PipelinePriority.First);
			GuardAgainstMultiple(steps, PipelinePriority.Last);

			steps.ToList().ForEach(item => _steps.Add((int)item.Priority, item));
		}

		public T Process(T dataToProcess)
		{
			foreach (var step in _steps)
			{
				try
				{
					dataToProcess = step.Value.Execute(dataToProcess);
				}
				catch (Exception ex)
				{
					throw new StepExecutionException(dataToProcess, step.GetType(), ex);
				}
			}

			return dataToProcess;
		}

		// REMARK: Guard Clause
		private void GuardAgainstMultiple(IPipelineStep<T>[] steps, PipelinePriority priority)
		{
			var name = Enum.GetName(typeof(PipelinePriority), priority);
			if (steps.Where(x => x.Priority == priority).Count() > 1)
			{
				throw new StepConfigurationException(string.Format("The pipeline cannot have multiple {0} steps", name));
			}
		}

		// REMARK: Guard Clause
		private void GuardAgainstNullSteps(IPipelineStep<T>[] steps)
		{
			if (steps == null || steps.Any(item => item == null))
			{
				throw new StepConfigurationException("The Pipeline cannot be configured with null steps");
			}
		}
	}
}