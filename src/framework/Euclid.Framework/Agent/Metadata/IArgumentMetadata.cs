namespace Euclid.Framework.Agent.Metadata
{
    public interface IArgumentMetadata : IPropertyMetadata
    {
        int Order { get; set; }
        object DefaultValue { get; set; }
    }
}