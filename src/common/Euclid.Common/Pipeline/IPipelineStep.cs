namespace Euclid.Common.Pipeline
{
	/// <summary>
	/// 	An operation that occurs as part of a pipeline of sequential steps.
	/// </summary>
	/// <typeparam name = "TPipelineState">The type of the state passed between pipeline steps. All steps in a pipeline must share a common state type.</typeparam>
	public interface IPipelineStep<TPipelineState>
	{
		/// <summary>
		/// 	Gets or sets the priority of this step within the pipeline.
		/// </summary>
		PipelinePriority Priority { get; set; }

		/// <summary>
		/// 	Executes this pipeline step.
		/// </summary>
		/// <param name = "input">The current state of the pipeline.</param>
		/// <returns>The pipeline state after modification by this step.</returns>
		TPipelineState Execute(TPipelineState input);
	}
}