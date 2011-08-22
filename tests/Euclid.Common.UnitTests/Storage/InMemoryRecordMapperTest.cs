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
		private RecordMapperTester<InMemoryRecordMapper<FakePublicationRecord>> _tester;

		[SetUp]
		public void Setup()
		{
			var repo = new InMemoryRecordMapper<FakePublicationRecord>();

			_tester = new RecordMapperTester<InMemoryRecordMapper<FakePublicationRecord>>(repo);
		}

		[Test]
		public void TestCreate()
		{
			_tester.TestCreate();
		}

		[Test]
		public void TestDelete()
		{
			_tester.TestDelete();
		}

		[Test]
		public void TestRetrieve()
		{
			_tester.TestRetrieve();
		}

		[Test]
		public void TestList()
		{
			_tester.TestList();
		}

		[Test]
		public void TestListPagination()
		{
			_tester.TestList();
		}

		[Test]
		public void TestUpdate()
		{
			_tester.TestUpdate();
		}
	}
}