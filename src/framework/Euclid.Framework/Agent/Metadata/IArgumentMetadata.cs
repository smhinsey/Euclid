namespace Euclid.Framework.Agent.Metadata
{
	public interface IArgumentMetadata : IPropertyMetadata
	{
		object DefaultValue { get; set; }
		int Order { get; set; }
	}
}