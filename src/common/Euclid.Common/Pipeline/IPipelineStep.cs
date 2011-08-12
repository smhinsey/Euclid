namespace Euclid.Common.Pipeline
{
	public interface IPipelineStep<T>
	{
		PipelinePriority Priority { get; set; }
		T Execute(T input);
	}
}