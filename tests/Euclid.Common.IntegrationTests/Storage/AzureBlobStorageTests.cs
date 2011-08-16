﻿using Euclid.Common.Storage.Azure;
using Euclid.Common.Storage.Configuration;
using Euclid.Common.UnitTests.Storage;
using Euclid.TestingSupport;
using Microsoft.WindowsAzure;
using NUnit.Framework;

namespace Euclid.Common.IntegrationTests.Storage
{
	[TestFixture]
	[Category(TestCategories.Integration)]
	public class AzureBlobStorageTests
	{
		#region Setup/Teardown

		[SetUp]
		public void Setup()
		{
			var settings = new BlobStorageSettings();

			var storageAccount = new CloudStorageAccount
				(CloudStorageAccount.DevelopmentStorageAccount.Credentials,
				 CloudStorageAccount.DevelopmentStorageAccount.BlobEndpoint,
				 CloudStorageAccount.DevelopmentStorageAccount.QueueEndpoint,
				 CloudStorageAccount.DevelopmentStorageAccount.TableEndpoint);

			var blobStorage = new AzureBlobStorage(storageAccount);
			blobStorage.Configure(settings);

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