using System;
using Euclid.Common.Registry;
using Euclid.Common.Serialization;
using Euclid.Common.Storage.Blob;
using Euclid.Common.Storage.Record;
using Euclid.Common.TestingFakes.Registry;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Registry
{
	[TestFixture]
	public class InMemoryRegistryTests
	{
		private RegistryTester<DefaultRecordRegistry<FakeRecord>> _registryTester;

		[TestFixtureSetUp]
		public void SetupTest()
		{
			var storage = new InMemoryBlobStorage();
			var serializer = new JsonMessageSerializer();
			var repository = new InMemoryRecordRepository<FakeRecord>();
			_registryTester =
				new RegistryTester<DefaultRecordRegistry<FakeRecord>>(new DefaultRecordRegistry<FakeRecord>(repository, storage, serializer));
		}

		private const int LargeNumber = 10000;
		private const int NumberThreads = 15;

		[Test]
		public void TestCreateRecord()
		{
			_registryTester.CreateRecord(new FakeMessage());
		}

		[Test]
		public void TestGetMessage()
		{
			var start = DateTime.Now;
			var createdById = new Guid("CBE5D20E-9B5A-46DF-B2FF-93B5F45A3460");
			var record = _registryTester.CreateRecord
				(new FakeMessage
				 	{
				 		Created = start,
				 		CreatedBy = createdById
				 	});

			var message = _registryTester.GetMessage(record);

			Assert.AreEqual(start.ToString(), message.Created.ToString());

			Assert.AreEqual(createdById, message.CreatedBy);
		}

		[Test]
		public void TestMarkAsCompleted()
		{
			_registryTester.MarkAsCompleted();
		}

		[Test]
		public void TestMarkAsFailed()
		{
			_registryTester.MarkAsFailed();
		}

		[Test]
		public void TestThroughputAsynchronously()
		{
			_registryTester.TestThroughputAsynchronously(LargeNumber, NumberThreads);
		}

		[Test]
		public void TestThroughputSynchronously()
		{
			_registryTester.TestThroughputSynchronously(LargeNumber);
		}
	}
}