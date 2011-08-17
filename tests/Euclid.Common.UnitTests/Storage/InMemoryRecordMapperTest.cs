﻿using Euclid.Common.Messaging;
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
		[SetUp]
		public void Setup()
		{
			var storage = new InMemoryBlobStorage();
			var serializer = new JsonMessageSerializer();
			var repo = new InMemoryRecordMapper<FakePublicationRecord>();

			_repoTester = new RecordMapperTester<InMemoryRecordMapper<FakePublicationRecord>>(repo);
		}

		private RecordMapperTester<InMemoryRecordMapper<FakePublicationRecord>> _repoTester;

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