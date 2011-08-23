using System.Collections.Generic;

namespace Euclid.Common.Storage
{
	/// <summary>
	/// A persistent binary large object. Typically, a serialized object graph, an image, or some other large file.
	/// </summary>
	public interface IBlob
	{
		/// <summary>
		/// Gets or sets the blob's content.
		/// </summary>
		byte[] Content { get; set; }

		/// <summary>
		/// Gets or sets the blob's content type.
		/// </summary>
		string ContentType { get; set; }

		/// <summary>
		/// Gets the blob's Etag. The Etag is derived from the blob's Content and changes whenever it does.
		/// </summary>
		string ETag { get; }

		/// <summary>
		/// Gets an MD5 hash of the blob's current content.
		/// </summary>
		string Md5 { get; }

		/// <summary>
		/// Gets metadata associated with the blob.
		/// </summary>
		IList<KeyValuePair<string, string>> Metdata { get; }
	}
}