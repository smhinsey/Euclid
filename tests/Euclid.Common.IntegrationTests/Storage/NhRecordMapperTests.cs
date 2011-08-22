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
			var cfg = new AutoMapperConfiguration(typeof(FakeMessage), typeof(FakePublicationRecord));

			this._session =
				Fluently.Configure().Database(SQLiteConfiguration.Standard.UsingFile("NhRecordMapperTests")).Mappings(
					map => map.AutoMappings.Add(AutoMap.AssemblyOf<FakeMessage>(cfg))).ExposeConfiguration(BuildSchema).
					BuildSessionFactory().OpenSession();
		}

		[SetUp]
		public void Setup()
		{
			if (this._session == null)
			{
				this.ConfigureDatabase();
			}

			var storage = new InMemoryBlobStorage();
			var serializer = new JsonMessageSerializer();
			var repo = new NhRecordMapper<FakePublicationRecord>(this._session);

			this._repoTester = new RecordMapperTester<NhRecordMapper<FakePublicationRecord>>(repo);
		}

		[Test]
		public void TestAutoMap()
		{
			Assert.Null(this._session.Query<FakeMessage>().FirstOrDefault());

			var id = Guid.NewGuid();
			var primaryKey = (Guid)this._session.Save(new FakeMessage { Created = DateTime.Now, CreatedBy = Guid.NewGuid() });
			Assert.NotNull(primaryKey);
			this._session.Flush();

			var message = this._session.Query<FakeMessage>().Where(m => m.Identifier == primaryKey).FirstOrDefault();
			Assert.NotNull(message);
			Assert.AreEqual(primaryKey, message.Identifier);
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

		private static void BuildSchema(NHibernate.Cfg.Configuration cfg)
		{
			new SchemaExport(cfg).Create(false, true);
		}
	}
}