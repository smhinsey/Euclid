using System;
using System.Linq;
using Euclid.Common.Serialization;
using Euclid.Common.Storage.Blob;
using Euclid.Common.Storage.Nhibernate;
using Euclid.Common.TestingFakes.Storage;
using Euclid.Common.UnitTests.Storage;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Euclid.Common.IntegrationTests.Storage
{
	public class NhibernateRecordRepositoryTest
	{
		private RecordRepositoryTester<NhibernateRecordRepository<FakeRecord>> _repoTester;
		private ISession _session;

		public void ConfigureDatabase()
		{
			var cfg = new MessageConfiguration();
			_session = Fluently
				.Configure()
				.Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("test-db")))
				.Mappings(map => map
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
			var repo = new NhibernateRecordRepository<FakeRecord>(serializer, storage, _session);

			_repoTester = new RecordRepositoryTester<NhibernateRecordRepository<FakeRecord>>(repo);
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

	public class MessageConfiguration : DefaultAutomappingConfiguration
	{
		public override bool IsId(Member member)
		{
			return (member.Name == "Identifier");
		}

		public override bool ShouldMap(Type type)
		{
			return type == typeof (FakeMessage)
			       || type == typeof (FakeRecord);
		}
	}
}