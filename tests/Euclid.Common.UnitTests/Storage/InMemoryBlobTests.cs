using Euclid.Common.Storage;
using Euclid.TestingSupport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Storage
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class InMemoryBlobTests
	{
		#region Setup/Teardown

		[SetUp]
		public void Setup()
		{
			var blobStorage = new InMemoryBlobStorage();

			blobStorage.Configure(new BlobStorageSettings());

			_blobTester = new BlobTester(blobStorage);
		}

		#endregion

		private BlobTester _blobTester;

		[Test]
		public void Deletes()
		{
			var blob = _blobTester.GetNewBlob();

			var uri = _blobTester.Put(blob);

			_blobTester.Delete(uri);

			var retrieved = _blobTester.Get(uri);

			Assert.IsNull(retrieved);
		}

		[Test]
		public void Gets()
		{
			var blob = _blobTester.GetNewBlob();

			var uri = _blobTester.Put(blob);

			var retrieved = _blobTester.Get(uri);

			Assert.AreEqual(blob.MD5, retrieved.MD5);

			Assert.AreEqual(blob.ContentType, retrieved.ContentType);

			Assert.AreEqual(blob.Metdata, retrieved.Metdata);

			Assert.False(string.IsNullOrEmpty(retrieved.ETag));
		}

		[Test]
		public void Puts()
		{
			var blob = _blobTester.GetNewBlob();

			_blobTester.Put(blob);
		}
	}
}