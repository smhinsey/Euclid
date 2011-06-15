using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Serialization;
using Euclid.Common.Storage.Blob;
using Euclid.Common.Storage.Registry;
using Euclid.Common.TestingFakes.Storage;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Storage
{
    [TestFixture]
    public class InMemoryRecordRepositoryTest
    {
        private RecordRepositoryTester<InMemoryRecordRepository<FakeRecord>> _repoTester;

        [SetUp]
        public void Setup()
        {
            var storage = new InMemoryBlobStorage();
            var serializer = new JsonMessageSerializer();
            var repo = new InMemoryRecordRepository<FakeRecord>(storage, serializer);

            _repoTester = new RecordRepositoryTester<InMemoryRecordRepository<FakeRecord>>(repo);   
        }

        [Test]
        public void TestCreate()
        {
            _repoTester.TestCreate();
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

        [Test]
        public void TestDelete()
        {
            _repoTester.TestDelete();
        }
    }
}
