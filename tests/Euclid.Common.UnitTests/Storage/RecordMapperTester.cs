﻿using System;
using Euclid.Common.Storage.Record;
using Euclid.Common.TestingFakes.Storage;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Storage
{
	public class RecordMapperTester<T> where T : IRecordMapper<FakePublicationRecord>
	{
		private readonly Type _fakeType = typeof (FakeMessage);
		private readonly Uri _fakeUri = new Uri("http://euclid.common.unittests.storage/fake/uri");
		private readonly T _repo;

		public RecordMapperTester(T repo)
		{
			_repo = repo;
		}

		public void TestCreate()
		{
			var r = _repo.Create(createFakeRecord());
			Assert.NotNull(r);
		}

		public void TestDelete()
		{
			var r = _repo.Create(createFakeRecord());

			var deleted = _repo.Delete(r.Identifier);
			Assert.NotNull(deleted);
			Assert.AreEqual(r.Identifier, deleted.Identifier);

			var retrieved = _repo.Retrieve(deleted.Identifier);
			Assert.Null(retrieved);
		}

		public void TestRetrieve()
		{
			var start = DateTime.Now;
			var r = _repo.Create(createFakeRecord());
			r.Created = start;

			var retrieved = _repo.Retrieve(r.Identifier);
			Assert.NotNull(retrieved);
			Assert.AreEqual(r.Identifier, retrieved.Identifier);
			Assert.AreEqual(r.Created, retrieved.Created);
		}

		public void TestUpdate()
		{
			var start = DateTime.Now;
			var r = _repo.Create(createFakeRecord());
			r.Created = start;

			var retrieved = _repo.Retrieve(r.Identifier);
			Assert.NotNull(retrieved);
			Assert.AreEqual(r.Identifier, retrieved.Identifier);
			Assert.AreEqual(r.Created, retrieved.Created);

			retrieved.Completed = true;
			var updated = _repo.Update(retrieved);
			Assert.NotNull(updated);
			Assert.AreEqual(true, updated.Completed);
		}

		private FakePublicationRecord createFakeRecord()
		{
			var record = new FakePublicationRecord
			             	{
			             		Created = DateTime.Now,
			             		Identifier = Guid.NewGuid(),
			             		MessageLocation = _fakeUri,
			             		MessageType = _fakeType
			             	};
			return record;
		}
	}
}