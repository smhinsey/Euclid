using System;
using Euclid.Common.Storage.Record;
using Euclid.Common.TestingFakes.Storage;
using NUnit.Framework;

namespace Euclid.Common.UnitTests.Storage
{
	public class RecordMapperTester<T>
		where T : IRecordMapper<FakePublicationRecord>
	{
		private readonly Type _fakeType = typeof(FakeMessage);

		private readonly Uri _fakeUri = new Uri("http://euclid.common.unittests.storage/fake/uri");

		private readonly T _mapper;

		public RecordMapperTester(T mapper)
		{
			_mapper = mapper;
		}

		public void TestCreate()
		{
			var r = _mapper.Create(createFakeRecord());
			Assert.NotNull(r);
		}

		public void TestDelete()
		{
			var r = _mapper.Create(createFakeRecord());

			var deleted = _mapper.Delete(r.Identifier);
			Assert.NotNull(deleted);
			Assert.AreEqual(r.Identifier, deleted.Identifier);

			var retrieved = _mapper.Retrieve(deleted.Identifier);
			Assert.Null(retrieved);
		}

		public void TestList()
		{
			for (var i = 0; i < 50; i++)
			{
				_mapper.Create(createFakeRecord());
			}

			var records = _mapper.List(50, 0);

			Assert.IsNotNull(records);
			Assert.AreEqual(50, records.Count);
		}

		public void TestListPagination()
		{
			for (var i = 0; i < 50; i++)
			{
				_mapper.Create(createFakeRecord());
			}

			var records = _mapper.List(10, 10);

			Assert.IsNotNull(records);
			Assert.AreEqual(10, records.Count);
		}

		public void TestRetrieve()
		{
			var start = DateTime.Now;
			var r = _mapper.Create(createFakeRecord());
			r.Created = start;

			var retrieved = _mapper.Retrieve(r.Identifier);
			Assert.NotNull(retrieved);
			Assert.AreEqual(r.Identifier, retrieved.Identifier);
			Assert.AreEqual(r.Created, retrieved.Created);
		}

		public void TestUpdate()
		{
			var start = DateTime.Now;
			var r = _mapper.Create(createFakeRecord());
			r.Created = start;

			var retrieved = _mapper.Retrieve(r.Identifier);
			Assert.NotNull(retrieved);
			Assert.AreEqual(r.Identifier, retrieved.Identifier);
			Assert.AreEqual(r.Created, retrieved.Created);

			retrieved.Completed = true;
			var updated = _mapper.Update(retrieved);
			Assert.NotNull(updated);
			Assert.AreEqual(true, updated.Completed);
		}

		private FakePublicationRecord createFakeRecord()
		{
			var record = new FakePublicationRecord
				{ Created = DateTime.Now, Identifier = Guid.NewGuid(), MessageLocation = _fakeUri, MessageType = _fakeType };
			return record;
		}
	}
}