using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Common.TestingFakes.Storage;
using Euclid.TestingSupport;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Storage
{
	[TestFixture]
	[Category(TestCategories.Unit)]
	public class InMemoryRecordMapperTest
	{
		private RecordMapperTester<InMemoryRecordMapper<FakePublicationRecord>> _repoTester;

		[SetUp]
		public void Setup()
		{
			var storage = new InMemoryBlobStorage();
			var serializer = new JsonMessageSerializer();
			var repo = new InMemoryRecordMapper<FakePublicationRecord>();

			this._repoTester = new RecordMapperTester<InMemoryRecordMapper<FakePublicationRecord>>(repo);
		}

		[Test]
		public void TestCreate()
		{
			this._repoTester.TestCreate();
		}

		[Test]
		public void TestDelete()
		{
			this._repoTester.TestDelete();
		}

		[Test]
		public void TestRetrieve()
		{
			this._repoTester.TestRetrieve();
		}

		[Test]
		public void TestUpdate()
		{
			this._repoTester.TestUpdate();
		}
	}
}