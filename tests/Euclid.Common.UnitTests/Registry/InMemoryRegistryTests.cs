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
			var repository = new InMemoryRecordRepository<FakeRecord>(storage, serializer);
			_registryTester =
				new RegistryTester<DefaultRecordRegistry<FakeRecord>>(new DefaultRecordRegistry<FakeRecord>(repository));
		}

		private const int LargeNumber = 10000;
		private const int NumberThreads = 15;

		[Test]
		public void TestCreateRecord()
		{
			_registryTester.CreateRecord(new FakeMessage());
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