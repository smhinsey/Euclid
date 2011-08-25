using System;
using System.Collections.Generic;
using System.Linq;

namespace Euclid.Common.Pipeline
{
	/// <summary>
	/// 	A pipeline encapsulates the sequential execution of a series of steps.
	/// </summary>
	/// <typeparam name = "TState">The of state which is passed between steps during execution.</typeparam>
	public class Pipeline<TState>
	{
		private readonly SortedList<int, IPipelineStep<TState>> _steps = new SortedList<int, IPipelineStep<TState>>();

		/// <summary>
		/// 	Configure installs a series of steps into the pipeline.
		/// </summary>
		/// <param name = "steps">The pipeline steps.</param>
		public void Configure(params IPipelineStep<TState>[] steps)
		{
			guardAgainstNullSteps(steps);
			guardAgainstMultiple(steps, PipelinePriority.First);
			guardAgainstMultiple(steps, PipelinePriority.Last);

			steps.ToList().ForEach(item => _steps.Add((int) item.Priority, item));
		}

		/// <summary>
		/// 	Process begins the sequential execution of the steps in the pipeline.
		/// </summary>
		/// <param name = "initialState">The initial state of the pipeline steps.</param>
		/// <returns>The final state after the last pipeline step has executed.</returns>
		public TState Process(TState initialState)
		{
			foreach (var step in _steps)
			{
				try
				{
					initialState = step.Value.Execute(initialState);
				}
				catch (Exception ex)
				{
					throw new StepExecutionException(initialState, step.GetType(), ex);
				}
			}

			return initialState;
		}

		private void guardAgainstMultiple(IPipelineStep<TState>[] steps, PipelinePriority priority)
		{
			var name = Enum.GetName(typeof (PipelinePriority), priority);
			if (steps.Where(x => x.Priority == priority).Count() > 1)
			{
				throw new StepConfigurationException(string.Format("The pipeline cannot have multiple {0} steps", name));
			}
		}

		private void guardAgainstNullSteps(IPipelineStep<TState>[] steps)
		{
			if (steps == null || steps.Any(item => item == null))
			{
				throw new StepConfigurationException("The Pipeline cannot be configured with null steps");
			}
		}
	}
}
