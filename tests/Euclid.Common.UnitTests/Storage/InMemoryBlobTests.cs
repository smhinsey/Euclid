using Euclid.Common.Storage;
using Euclid.TestingSupport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Storage
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class InMemoryBlobTests
	{
		private BlobTester _blobTester;

		[Test]
		public void Deletes()
		{
			var blob = this._blobTester.GetNewBlob();

			var uri = this._blobTester.Put(blob);

			this._blobTester.Delete(uri);

			var retrieved = this._blobTester.Get(uri);

			Assert.IsNull(retrieved);
		}

		[Test]
		public void Gets()
		{
			var blob = this._blobTester.GetNewBlob();

			var uri = this._blobTester.Put(blob);

			var retrieved = this._blobTester.Get(uri);

			Assert.AreEqual(blob.MD5, retrieved.MD5);

			Assert.AreEqual(blob.ContentType, retrieved.ContentType);

			Assert.AreEqual(blob.Metdata, retrieved.Metdata);

			Assert.False(string.IsNullOrEmpty(retrieved.ETag));
		}

		[Test]
		public void Puts()
		{
			var blob = this._blobTester.GetNewBlob();

			this._blobTester.Put(blob);
		}

		[SetUp]
		public void Setup()
		{
			var blobStorage = new InMemoryBlobStorage();

			blobStorage.Configure(new BlobStorageSettings());

			this._blobTester = new BlobTester(blobStorage);
		}
	}
}