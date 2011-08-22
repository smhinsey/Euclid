using System;
using System.Collections.Concurrent;
using Euclid.Common.Extensions;
using Euclid.Common.Storage.Binary;

namespace Euclid.Common.Storage
{
	public class InMemoryBlobStorage : IBlobStorage
	{
		private readonly ConcurrentDictionary<Uri, IBlob> _blobs;

		private string _containerName;

		public InMemoryBlobStorage()
		{
			this._blobs = new ConcurrentDictionary<Uri, IBlob>();
		}

		public void Configure(IBlobStorageSettings settings)
		{
			this._containerName = settings.ContainerName.Value;
		}

		public void Delete(Uri uri)
		{
			if (this.Exists(uri))
			{
				IBlob blob;
				this._blobs.TryRemove(uri, out blob);
			}
		}

		public bool Exists(Uri uri)
		{
			return this._blobs.ContainsKey(uri);
		}

		public IBlob Get(Uri uri)
		{
			IBlob blob = null;

			if (this.Exists(uri))
			{
				this._blobs.TryGetValue(uri, out blob);
			}

			return blob;
		}

		public Uri Put(IBlob blob, string name)
		{
			var upload = new Blob(blob);

			var uri =
				new Uri(
					string.Format(
						"http://in-memory/{0}/{1}/{2}.{3}", 
						this._containerName, 
						Guid.NewGuid(), 
						name, 
						MimeTypes.GetExtensionFromContentType(blob.ContentType)));

			this._blobs.TryAdd(uri, upload);

			return uri;
		}
	}
}