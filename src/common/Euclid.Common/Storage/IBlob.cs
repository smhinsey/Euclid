using System.Collections.Generic;

namespace Euclid.Common.Storage
{
	public interface IBlob
	{
		byte[] Bytes { get; set; }

		string ContentType { get; set; }

		string ETag { get; }

		string MD5 { get; }

		IList<KeyValuePair<string, string>> Metdata { get; }
	}
}