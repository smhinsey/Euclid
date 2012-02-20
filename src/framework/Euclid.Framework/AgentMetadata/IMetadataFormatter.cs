using System.Collections.Generic;
using System.Text;

namespace Euclid.Framework.AgentMetadata
{
	public interface IMetadataFormatter
	{
		string GetContentType(string format);

		Encoding GetEncoding(string format);

		IEnumerable<string> GetFormats(string contentType);

		string GetRepresentation(string format);
		object GetJsonObject();
	}
}