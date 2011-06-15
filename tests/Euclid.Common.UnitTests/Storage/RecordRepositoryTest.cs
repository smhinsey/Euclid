using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Storage;
using Euclid.Common.TestingFakes.Storage;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Storage
{
    public class RecordRepositoryTester<T> where T : IBasicRecordRepository<FakeRecord>
    {
        private readonly T _repo;
        public RecordRepositoryTester(T repo)
        {
            _repo = repo;
        }

        public void TestCreate()
        {
            var r = _repo.Create(new FakeMessage());
            Assert.NotNull(r);
        }

        public void TestRetrieve()
        {
            var start = DateTime.Now;
            var r = _repo.Create(new FakeMessage { Created = start });

            var retrieved = _repo.Retrieve(r.Identifier);

            Assert.NotNull(retrieved);
            Assert.AreEqual(r.Identifier, retrieved.Identifier);
            Assert.AreEqual(r.Created, retrieved.Created);
        }

        public void TestUpdate()
        {
            var start = DateTime.Now;
            var r = _repo.Create(new FakeMessage { Created = start });

            var retrieved = _repo.Retrieve(r.Identifier);
            Assert.NotNull(retrieved);
            Assert.AreEqual(r.Identifier, retrieved.Identifier);
            Assert.AreEqual(r.Created, retrieved.Created);

            retrieved.Completed = true;
            var updated = _repo.Update(retrieved);
            Assert.NotNull(updated);
            Assert.AreEqual(true, updated.Completed);
        }

        public void TestDelete()
        {
            var r = _repo.Create(new FakeMessage());

            var deleted = _repo.Delete(r.Identifier);
            Assert.NotNull(deleted);
            Assert.AreEqual(r.Identifier, deleted.Identifier);

            var retrieved = _repo.Retrieve(deleted.Identifier);
            Assert.Null(retrieved);
        }

    }
}
