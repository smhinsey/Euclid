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
			_blobs = new ConcurrentDictionary<Uri, IBlob>();
		}

		public void Configure(IBlobStorageSettings settings)
		{
			_containerName = settings.ContainerName.Value;
		}

		public void Delete(Uri uri)
		{
			if (Exists(uri))
			{
				IBlob blob;
				_blobs.TryRemove(uri, out blob);
			}
		}

		public bool Exists(Uri uri)
		{
			return _blobs.ContainsKey(uri);
		}

		public IBlob Get(Uri uri)
		{
			IBlob blob = null;

			if (Exists(uri))
			{
				_blobs.TryGetValue(uri, out blob);
			}

			return blob;
		}

		public Uri Put(IBlob blob, string name)
		{
			var upload = new Blob(blob);

			var uri = new Uri(string.Format("http://in-memory/{0}/{1}/{2}.{3}", _containerName, Guid.NewGuid(), name, MimeTypes.GetExtensionFromContentType(blob.ContentType)));

			_blobs.TryAdd(uri, upload);

			return uri;
		}
	}
}