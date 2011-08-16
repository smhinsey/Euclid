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
		private RecordMapperTester<NhRecordMapper<FakePublicationRecord>> _repoTester;
		private ISession _session;

		public void ConfigureDatabase()
		{
			var cfg = new AutoMapperConfiguration(typeof (FakeMessage), typeof(FakePublicationRecord));

			_session = Fluently
				.Configure()
				.Database(SQLiteConfiguration.Standard.UsingFile("NhRecordMapperTests"))
				.Mappings
				(map => map
				        	.AutoMappings
				        	.Add(AutoMap.AssemblyOf<FakeMessage>(cfg)))
				.ExposeConfiguration(BuildSchema)
				.BuildSessionFactory()
				.OpenSession();
		}

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

			_repoTester = new RecordMapperTester<NhRecordMapper<FakePublicationRecord>>(repo);
		}

		[Test]
		public void TestAutoMap()
		{
			Assert.Null(_session.Query<FakeMessage>().FirstOrDefault());

			var id = Guid.NewGuid();
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

		private static void BuildSchema(NHibernate.Cfg.Configuration cfg)
		{
			new SchemaExport(cfg).Create(false, true);
		}
	}
}