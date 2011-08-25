using System;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Common.TestingFakes.Registry;
using Euclid.TestingSupport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Registry
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class InMemoryRegistryTests
	{
		private const int LargeNumber = 10000;

		private const int NumberThreads = 15;

		private RegistryTester<PublicationRegistry<FakePublicationRecord, FakePublicationRecord>> _registryTester;

		[TestFixtureSetUp]
		public void SetupTest()
		{
			var storage = new InMemoryBlobStorage();
			var serializer = new JsonMessageSerializer();
			var repository = new InMemoryRecordMapper<FakePublicationRecord>();
			_registryTester =
				new RegistryTester<PublicationRegistry<FakePublicationRecord, FakePublicationRecord>>(
					new PublicationRegistry<FakePublicationRecord, FakePublicationRecord>(repository, storage, serializer));
		}

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
			var record = _registryTester.CreateRecord(new FakeMessage {Created = start, CreatedBy = createdById});

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

		[Test]
		public void TestUnableToDispatch()
		{
			_registryTester.MarkAsUnableToDispatch();
		}
	}
}
