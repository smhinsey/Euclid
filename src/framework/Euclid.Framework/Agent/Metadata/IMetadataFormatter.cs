using System.Collections.Generic;
using System.Text;

namespace Euclid.Framework.Agent.Metadata
{
    public interface IMetadataFormatter
    {
        string GetContentType(string format);
        IEnumerable<string> GetFormats(string contentType);
        string GetRepresentation(string format);
        Encoding GetEncoding(string format);
    }
}