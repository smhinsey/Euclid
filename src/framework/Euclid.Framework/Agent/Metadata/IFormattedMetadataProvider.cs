using System.Collections.Generic;

namespace Euclid.Framework.Agent.Metadata
{
    public interface IFormattedMetadataProvider
    {
        string GetContentType(string format);
        IEnumerable<string> GetFormats(string contentType);
        string GetRepresentation(string format);
    }
}