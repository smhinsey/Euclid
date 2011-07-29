namespace Euclid.Framework.Cqrs.Metadata
{
	public interface IAgentMetadata
	{
		string CommandNamespace { get; set; }
		string CommandProcessorNamespace { get; set; }
		string FriendlyName { get; set; }
		string QueryNamespace { get; set; }
		string Scheme { get; set; }
		string SystemName { get; set; }
	}
}