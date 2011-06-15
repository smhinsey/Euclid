using Euclid.Common.Serialization;
using Euclid.Common.Storage.Blob;
using Euclid.Common.Storage.Record;
using Euclid.Common.TestingFakes.Storage;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Storage
{
	[TestFixture]
	public class InMemoryRecordRepositoryTest
	{
		#region Setup/Teardown

		[SetUp]
		public void Setup()
		{
			var storage = new InMemoryBlobStorage();
			var serializer = new JsonMessageSerializer();
			var repo = new InMemoryRecordRepository<FakeRecord>(storage, serializer);

			_repoTester = new RecordRepositoryTester<InMemoryRecordRepository<FakeRecord>>(repo);
		}

		#endregion

		private RecordRepositoryTester<InMemoryRecordRepository<FakeRecord>> _repoTester;

		[Test]
		public void TestCreate()
		{
			_repoTester.TestCreate();
		}

		[Test]
		public void TestDelete()
		{
			_repoTester.TestDelete();
		}

		[Test]
		public void TestRetrieve()
		{
			_repoTester.TestRetrieve();
		}

		[Test]
		public void TestUpdate()
		{
			_repoTester.TestUpdate();
		}
	}
}