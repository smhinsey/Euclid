using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Euclid.Common.Extensions;

namespace Euclid.Common.Storage.Blob
{
	public class InMemoryBlobStorage : IBlobStorage
	{
		private readonly ConcurrentDictionary<string, byte[]> _blobs;

		public InMemoryBlobStorage()
		{
			_blobs = new ConcurrentDictionary<string, byte[]>();
		}

		public void Delete(string uri)
		{
			if (Exists(uri))
			{
				byte[] blob;
				_blobs.TryRemove(uri, out blob);
			}
		}

		public void Delete(Uri uri)
		{
			Delete(uri.ToString());
		}

		public bool Exists(Uri uri)
		{
			return Exists(uri.ToString());
		}

		public bool Exists(string uri)
		{
			return _blobs.ContainsKey(uri);
		}

		public byte[] Get(Uri uri)
		{
			return Get(uri.ToString());
		}

		public byte[] Get(string uri)
		{
			byte[] bytes = null;
			if (Exists(uri))
			{
				_blobs.TryGetValue(uri, out bytes);
			}

			return bytes;
		}

		public Uri Put(Stream blob, string name, IList<KeyValuePair<string, string>> metadata)
		{
			var uri = new Uri(string.Format("http://in-memory/blob/{0}", Guid.NewGuid()));

			_blobs.TryAdd(uri.ToString(), blob.GetBytes());

			return uri;
		}
	}
}