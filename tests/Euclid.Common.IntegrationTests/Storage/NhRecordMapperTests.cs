using System;
using System.Linq;
using Euclid.Common.Messaging;
using Euclid.Common.Storage;
using Euclid.Common.Storage.NHibernate;
using Euclid.Common.TestingFakes.Storage;
using Euclid.Common.UnitTests.Storage;
using Euclid.TestingSupport;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Euclid.Common.IntegrationTests.Storage
{
	[TestFixture]
	[Category(TestCategories.Integration)]
	public class NhRecordMapperTests
	{
		#region Setup/Teardown

		[SetUp]
		public void Setup()
		{
			if (_session == null)
			{
				ConfigureDatabase();
			}

			var storage = new InMemoryBlobStorage();
			var serializer = new JsonMessageSerializer();
			var repo = new NhRecordMapper<FakePublicationRecord>(_session);

			_tester = new RecordMapperTester<NhRecordMapper<FakePublicationRecord>>(repo);
		}

		#endregion

		private ISession _session;

		private RecordMapperTester<NhRecordMapper<FakePublicationRecord>> _tester;

		public void ConfigureDatabase()
		{
			var cfg = new AutoMapperConfiguration(typeof (FakeMessage), typeof (FakePublicationRecord));

			_session =
				Fluently.Configure().Database(SQLiteConfiguration.Standard.UsingFile("NhRecordMapperTests")).Mappings(
				                                                                                                      map =>
				                                                                                                      map.
				                                                                                                      	AutoMappings.
				                                                                                                      	Add(
				                                                                                                      	    AutoMap.
				                                                                                                      	    	AssemblyOf
				                                                                                                      	    	<
				                                                                                                      	    	FakeMessage
				                                                                                                      	    	>(cfg)))
					.ExposeConfiguration(buildSchema).
					BuildSessionFactory().OpenSession();
		}

		private static void buildSchema(NHibernate.Cfg.Configuration cfg)
		{
			new SchemaExport(cfg).Create(false, true);
		}

		[Test]
		public void TestAutoMap()
		{
			Assert.Null(_session.Query<FakeMessage>().FirstOrDefault());

			var primaryKey = (Guid) _session.Save(new FakeMessage {Created = DateTime.Now, CreatedBy = Guid.NewGuid()});
			Assert.NotNull(primaryKey);
			_session.Flush();

			var message = _session.Query<FakeMessage>().Where(m => m.Identifier == primaryKey).FirstOrDefault();
			Assert.NotNull(message);
			Assert.AreEqual(primaryKey, message.Identifier);
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
		public void TestRetrieve()
		{
			_tester.TestRetrieve();
		}

		[Test]
		public void TestUpdate()
		{
			_tester.TestUpdate();
		}
	}
}
