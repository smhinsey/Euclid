namespace Euclid.Framework.Cqrs.Metadata
{
    public interface IAgentMetadata
    {
        string FriendlyName { get; set; }
        string SystemName { get; set; }
        string Scheme { get; set; }
        string CommandNamespace { get; set; }
        string QueryNamespace { get; set; }
        string CommandProcessorNamespace { get; set; }
    }
}