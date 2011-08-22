using System;
using System.Text;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Storage
{
	public class BlobTester
	{
		private readonly IBlobStorage _blobStorage;

		public BlobTester(IBlobStorage blobStorage)
		{
			this._blobStorage = blobStorage;
		}

		public Uri Delete(Uri uri)
		{
			Assert.NotNull(uri);
			Assert.True(this._blobStorage.Exists(uri));

			this._blobStorage.Delete(uri);
			Assert.False(this._blobStorage.Exists(uri));

			return uri;
		}

		public bool Exists(Uri uri)
		{
			Assert.NotNull(uri);
			return this._blobStorage.Exists(uri);
		}

		public IBlob Get(Uri uri)
		{
			Assert.NotNull(uri);

			var retrieved = this._blobStorage.Get(uri);
			return retrieved;
		}

		public IBlob GetNewBlob()
		{
			return new Blob
				{
					Bytes =
						Encoding.UTF8.GetBytes(
							string.Format(
								"<blob><title>Test Blob</title><created>{0}</created><testing>{1}</testing></blob>", 
								DateTime.Now, 
								this._blobStorage.GetType().FullName)), 
					ContentType = "text/xml", 
				};
		}

		public Uri Put(IBlob blob)
		{
			Assert.NotNull(blob);
			Assert.NotNull(blob.Bytes);

			var uri = this._blobStorage.Put(blob, "test");

			Assert.NotNull(uri);

			return uri;
		}
	}
}