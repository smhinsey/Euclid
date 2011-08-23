using System;
using Euclid.Common.Logging;

namespace Euclid.Common.Storage.Binary
{
	/// <summary>Persists instance of IBlobs to a remote persistence engine such as Azure Binary Storage or Amazon S3.</summary>
	public interface IBlobStorage : ILoggingSource
	{
		/// <summary>Configure the storage engine.</summary>
		/// <param name="settings">The settings required to interact with the storage engine.</param>
		void Configure(IBlobStorageSettings settings);

		/// <summary>Deletes a blob by URI.</summary>
		/// <param name="uri">The URI of the blob to be deleted.</param>
		void Delete(Uri uri);

		/// <summary>Determines if a blob exists based on its URI.</summary>
		/// <param name="uri">The URI to verify.</param>
		/// <returns>Whether or not a blob exists at the supplied URI.</returns>
		bool Exists(Uri uri);

		/// <summary>Returns a blob based on its URI.</summary>
		/// <param name="uri">The URI of the blob to return.</param>
		/// <returns>The blob which exists at the supplied URI.</returns>
		IBlob Get(Uri uri);

		/// <summary>Stores a blob with the storage engine and returns its URI.</summary>
		/// <param name="blob">The blob to be stored.</param>
		/// <param name="name">The name of the blob as if it were a file but without the extension.</param>
		/// <returns>The blob's URI.</returns>
		Uri Put(IBlob blob, string name);
		
		// SELF i think it might make sense to move name to IBlob
	}
}