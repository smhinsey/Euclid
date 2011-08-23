namespace Euclid.Common.Pipeline
{
	/// <summary>
	/// Indicates the priority of an step in a pipeline.
	/// </summary>
	public enum PipelinePriority
	{
		/// <summary>
		/// Indicates that the step must execute before all other steps.
		/// </summary>
		First = -1, 

		/// <summary>
		/// Indicates that the priority of the step is low.
		/// </summary>
		Low = 0, 

		/// <summary>
		/// Indicates that the priority of the step is normal.
		/// </summary>
		Normal = 10, 

		/// <summary>
		/// Indicates that the priority of the step is high.
		/// </summary>
		High = 100, 

		/// <summary>
		/// Indicates that the step must execute after all other steps.
		/// </summary>
		Last = 1000
	}
}