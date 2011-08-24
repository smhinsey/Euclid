namespace Euclid.Framework.AgentMetadata
{
	public interface IArgumentMetadata : IPropertyMetadata
	{
		object DefaultValue { get; set; }

		int Order { get; set; }
	}
}
