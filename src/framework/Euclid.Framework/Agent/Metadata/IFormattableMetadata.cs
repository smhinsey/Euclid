namespace Euclid.Framework.Agent.Metadata
{
	public interface IFormattableMetadata : ITypeMetadata
	{
		IMetadataFormatter Formatter { get; }
	}
}